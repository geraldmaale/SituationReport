using GreatIdeas.Repository;
using Mapster;
using Microsoft.EntityFrameworkCore;
using SR.Data.Repositories.Contracts;
using SR.Shared.DTOs.Operations;
using SR.Shared.Entities;

namespace SR.Data.Repositories.Persistence;
public class OperationProcessingRepository: RepositoryFactory<ApplicationDbContext, OperationInspection, OperationInspectionDto>,
	IOperationProcessingRepository
{
	public OperationProcessingRepository(IDbContextFactory<ApplicationDbContext> dbContextFactory) : base(dbContextFactory)
	{
	}

	public async ValueTask<IEnumerable<OperationInspectionDto>> GetAllAsync(Guid operationOfficeId, CancellationToken cancellationToken)
	{
		await using var dbContext = await DbContextFactory.CreateDbContextAsync(cancellationToken);
		var query = dbContext.Set<OperationInspection>()
			.Include(e => e.OperationInspectionDetails)
			.ThenInclude(e => e.OperationType)
			.Include(e => e.Officer)
			.AsTracking()
			.Where(x => x.OperationOfficeId == operationOfficeId)
			.ProjectToType<OperationInspectionDto>();
			
		return await query.ToListAsync(cancellationToken);
	}
	
	public async ValueTask<OperationInspection> GetById(Guid operationOfficeId, Guid id, CancellationToken cancellationToken)
	{
		await using var dbContext = await DbContextFactory.CreateDbContextAsync(cancellationToken);
		var query = dbContext.Set<OperationInspection>()
			.Include(e => e.OperationInspectionDetails)
			.ThenInclude(e => e.OperationType)
			.Include(e => e.Officer)
			.AsQueryable();
		
		var result = await query.FirstOrDefaultAsync(
			x=>x.Id == id && x.OperationOfficeId == operationOfficeId, cancellationToken);
		
		return result!;
	}
	
	public async ValueTask DeleteExistingRecords(OperationInspection model, CancellationToken cancellationToken)
	{
		await using var dbContext = await DbContextFactory.CreateDbContextAsync(cancellationToken);
		dbContext.OperationInspectionDetails.RemoveRange(model.OperationInspectionDetails);
		await dbContext.SaveChangesAsync(cancellationToken);
	}
}
