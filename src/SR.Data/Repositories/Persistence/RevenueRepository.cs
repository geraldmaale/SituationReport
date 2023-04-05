using GreatIdeas.Repository;
using Mapster;
using Microsoft.EntityFrameworkCore;
using SR.Data.Repositories.Contracts;
using SR.Shared.DTOs.Revenues;
using SR.Shared.Entities;
using SR.Shared.Params;

namespace SR.Data.Repositories.Persistence;
public class RevenueRepository : RepositoryFactory<ApplicationDbContext, RevenueCollection, RevenueCollectionDto>, IRevenueRepository
{
	private readonly ExportFileHelper _exportFileHelper;
	public RevenueRepository(IDbContextFactory<ApplicationDbContext> dbContextFactory, ExportFileHelper exportFileHelper) : base(dbContextFactory)
	{
		_exportFileHelper = exportFileHelper;
	}

	public async ValueTask<RevenueCollection> GetById(Guid id, CancellationToken cancellationToken)
	{
		await using var dbContext = await DbContextFactory.CreateDbContextAsync(cancellationToken);
		var query = dbContext.Set<RevenueCollection>()
			.Include(e => e.RevenueCollectionDetails)
			.ThenInclude(e => e.RevenueType)
			.Include(e => e.Officer)
			.AsQueryable();

		var result = await query.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

		return result!;
	}

	public async ValueTask<List<RevenueCollectionDetailExportDto>> GetRevenueCollectionDetails(PagingParameters pagingParams)
	{
		await using var dbContext = await DbContextFactory.CreateDbContextAsync();
		var queryable = await dbContext.RevenueCollectionDetails
			.AsNoTracking()
			.Include(x=>x.RevenueType)
			.Include(x=>x.RevenueCollection)
			.ToListAsync();

		var collections = queryable.AsQueryable();

		if (pagingParams.StartDate != null && pagingParams.EndDate != null)
		{
			var startDate = DateTime.SpecifyKind(pagingParams.StartDate.Value.Date, DateTimeKind.Utc);
			var endDate = DateTime.SpecifyKind(pagingParams.EndDate.Value.Date, DateTimeKind.Utc);

			collections = collections.Where(m => m.RevenueCollection.EntryDate.Date >= (startDate)
			                                     && m.RevenueCollection.EntryDate.Date <= (endDate));
		}

		// Group by revenue type
		var groupedResults = collections.GroupBy(x => x.RevenueType.Name);

		var revenueCollectionDetails = new List<RevenueCollectionDetailExportDto>();
		foreach (var revenue in groupedResults)
		{
			revenueCollectionDetails.Add(new RevenueCollectionDetailExportDto
			{
				Revenue = revenue.Key,
				Amount = revenue.Sum(x => x.Amount)
			});
		}

		return revenueCollectionDetails;
	}

	public async ValueTask DeleteRevenueCollectionDetail(Guid revenueCollectionDetailId, Guid revenueCollectionId, CancellationToken cancellationToken)
	{
		await using var dbContext = await DbContextFactory.CreateDbContextAsync(cancellationToken);
		var query = await dbContext.Set<RevenueCollectionDetail>()
			.FirstOrDefaultAsync(
				x => x.RevenueCollectionDetailId == revenueCollectionDetailId
				&& x.RevenueCollectionId == revenueCollectionId, cancellationToken);

		dbContext.RevenueCollectionDetails.Remove(query!);
		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async ValueTask<MemoryStream> Export(PagingParameters pagingParams)
	{
		var collections = await Filter(pagingParams);
		var results = collections.ProjectToType<RevenueCollectionDto>().ToList();

		var exporter = _exportFileHelper.GenerateExcel<RevenueCollectionDto>(results, pagingParams.Title, pagingParams.ExportType, true);

		return exporter;
	}

	private async ValueTask<IQueryable<RevenueCollection>> Filter(PagingParameters pagingParams)
	{
		await using var dbContext = await DbContextFactory.CreateDbContextAsync();
		var collections = dbContext.Set<RevenueCollection>().AsNoTracking().AsQueryable();


		if (pagingParams.StartDate != null && pagingParams.EndDate != null)
		{
			var startDate = DateTime.SpecifyKind(pagingParams.StartDate.Value, DateTimeKind.Utc);
			var endDate = DateTime.SpecifyKind(pagingParams.EndDate.Value, DateTimeKind.Utc);

			collections = collections.Where(m => m.EntryDate >= (startDate)
												 && m.EntryDate <= (endDate));
		}

		return collections;
	}
}
