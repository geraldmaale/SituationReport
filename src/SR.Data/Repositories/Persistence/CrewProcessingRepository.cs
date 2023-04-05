using GreatIdeas.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using SR.Data.Repositories.Contracts;
using SR.Shared.DTOs.Crews;
using SR.Shared.DTOs.Revenues;
using SR.Shared.Entities;
using SR.Shared.Params;

namespace SR.Data.Repositories.Persistence;
public class CrewProcessingRepository :  RepositoryFactory<ApplicationDbContext, CrewProcessing, CrewProcessingDto>, ICrewProcessingRepository
{
	public CrewProcessingRepository(IDbContextFactory<ApplicationDbContext> dbContextFactory) : base(dbContextFactory)
	{
	}
	
	public async ValueTask<CrewProcessing> GetById(Guid id, CancellationToken cancellationToken)
	{
		await using var dbContext = await DbContextFactory.CreateDbContextAsync(cancellationToken);
		var query = await dbContext.Set<CrewProcessing>()
			.Include(e=>e.Embarkations)
			.ThenInclude(e=>e.Nationality)
			.Include(x=>x.DisEmbarkations)
			.ThenInclude(x=>x.Nationality)
			.Include(x=>x.VesselsDocked)
			.ThenInclude(x=>x.VesselType)
			.Include(x=>x.AshorePasses)
			.ThenInclude(x=>x.PassType)
			.AsNoTracking()
			.FirstOrDefaultAsync(x=>x.Id == id, cancellationToken);
		
		return query!;
	}
	
	public async ValueTask<List<EmbarkationExportDto>> GetEmbarkationDetails(PagingParameters pagingParams)
	{
		DateTime startDate = default;
		DateTime endDate = default;
		if (pagingParams.StartDate != null && pagingParams.EndDate != null)
		{
			startDate = DateTime.SpecifyKind(pagingParams.StartDate.Value.Date, DateTimeKind.Utc);
			endDate = DateTime.SpecifyKind(pagingParams.EndDate.Value.Date, DateTimeKind.Utc);
		}
		
		await using var dbContext = await DbContextFactory.CreateDbContextAsync();
		
		var embarkations = await dbContext.Embarkations
			.AsNoTracking()
			.Include(e=>e.CrewProcessing)
			.Where(m => m.CrewProcessing.EntryDate.Date >= (startDate)
			            && m.CrewProcessing.EntryDate.Date <= (endDate))
			.ToListAsync();

		// Group by embarkation and disembarkation
		var groupedEmbarkations = embarkations.GroupBy(x => x.Nationality.Name);
		
		// Create new objects
		var embarkationExports = new List<EmbarkationExportDto>();
		foreach (var embarkation in groupedEmbarkations)
		{
			embarkationExports.Add(new EmbarkationExportDto
			{
				Nationality = embarkation.Key,
				Number = embarkation.Sum(x => x.Count)
			});
		}
		
		return embarkationExports;
	}
	
	public async ValueTask<List<DisEmbarkationExportDto>> GetDisEmbarkationDetails(PagingParameters pagingParams)
	{
		DateTime startDate = default;
		DateTime endDate = default;
		if (pagingParams.StartDate != null && pagingParams.EndDate != null)
		{
			startDate = DateTime.SpecifyKind(pagingParams.StartDate.Value.Date, DateTimeKind.Utc);
			endDate = DateTime.SpecifyKind(pagingParams.EndDate.Value.Date, DateTimeKind.Utc);
		}
		
		await using var dbContext = await DbContextFactory.CreateDbContextAsync();
		
		var embarkations = await dbContext.DisEmbarkations
			.AsNoTracking()
			.Include(e=>e.CrewProcessing)
			.Where(m => m.CrewProcessing.EntryDate.Date >= (startDate)
			            && m.CrewProcessing.EntryDate.Date <= (endDate))
			.ToListAsync();

		// Group by embarkation and disembarkation
		var groupedResults = embarkations.GroupBy(x => x.Nationality.Name);
		
		// Create new objects
		var disEmbarkationExports = new List<DisEmbarkationExportDto>();
		foreach (var disembarkation in groupedResults)
		{
			disEmbarkationExports.Add(new ()
			{
				Nationality = disembarkation.Key,
				Number = disembarkation.Sum(x => x.Count)
			});
		}
		
		return disEmbarkationExports;
	}
	
	public async ValueTask<List<VesselDockedExportDto>> GetVesselsDockedDetails(PagingParameters pagingParams)
	{
		DateTime startDate = default;
		DateTime endDate = default;
		if (pagingParams.StartDate != null && pagingParams.EndDate != null)
		{
			startDate = DateTime.SpecifyKind(pagingParams.StartDate.Value.Date, DateTimeKind.Utc);
			endDate = DateTime.SpecifyKind(pagingParams.EndDate.Value.Date, DateTimeKind.Utc);
		}
		
		await using var dbContext = await DbContextFactory.CreateDbContextAsync();
		
		var vesselsDocked = await dbContext.VesselsDocked
			.AsNoTracking()
			.Include(e=>e.CrewProcessing)
			.Where(m => m.CrewProcessing.EntryDate.Date >= (startDate)
			            && m.CrewProcessing.EntryDate.Date <= (endDate))
			.ToListAsync();

		// Group by embarkation and disembarkation
		var groupedResults = vesselsDocked.GroupBy(x => x.VesselType.Name);
		
		// Create new objects
		var disEmbarkationExports = new List<VesselDockedExportDto>();
		foreach (var vessel in groupedResults)
		{
			disEmbarkationExports.Add(new ()
			{
				Vessel = vessel.Key,
				Number = vessel.Sum(x => x.Count)
			});
		}
		
		return disEmbarkationExports;
	}
	
	public async ValueTask<List<AshorePassExportDto>> GetAshorePassDetails(PagingParameters pagingParams)
	{
		DateTime startDate = default;
		DateTime endDate = default;
		if (pagingParams.StartDate != null && pagingParams.EndDate != null)
		{
			startDate = DateTime.SpecifyKind(pagingParams.StartDate.Value.Date, DateTimeKind.Utc);
			endDate = DateTime.SpecifyKind(pagingParams.EndDate.Value.Date, DateTimeKind.Utc);
		}
		
		await using var dbContext = await DbContextFactory.CreateDbContextAsync();
		
		var vesselsDocked = await dbContext.AshorePasses
			.AsNoTracking()
			.Include(e=>e.CrewProcessing)
			.Where(m => m.CrewProcessing.EntryDate.Date >= (startDate)
			            && m.CrewProcessing.EntryDate.Date <= (endDate))
			.ToListAsync();

		// Group by embarkation and disembarkation
		var groupedResults = vesselsDocked.GroupBy(x => x.PassType.Name);
		
		// Create new objects
		var ashorePassExports = new List<AshorePassExportDto>();
		foreach (var ashorePass in groupedResults)
		{
			ashorePassExports.Add(new ()
			{
				AshorePass = ashorePass.Key,
				Number = ashorePass.Sum(x => x.NumberOfPass)
			});
		}
		
		return ashorePassExports;
	}
	
	public async ValueTask<CrewProcessingExportDto> GetCrewProcessingDetails(PagingParameters pagingParams)
	{
		DateTime startDate = default;
		DateTime endDate = default;
		if (pagingParams.StartDate != null && pagingParams.EndDate != null)
		{
			startDate = DateTime.SpecifyKind(pagingParams.StartDate.Value.Date, DateTimeKind.Utc);
			endDate = DateTime.SpecifyKind(pagingParams.EndDate.Value.Date, DateTimeKind.Utc);
		}
		
		await using var dbContext = await DbContextFactory.CreateDbContextAsync();
		
		var embarkations = await dbContext.Embarkations
			.AsNoTracking()
			.Include(e=>e.CrewProcessing)
			.Where(m => m.CrewProcessing.EntryDate.Date >= (startDate)
			            && m.CrewProcessing.EntryDate.Date <= (endDate))
			.ToListAsync();
		
		var disEmbarkations = await dbContext.DisEmbarkations
			.Include(e=>e.CrewProcessing)
			.AsNoTracking()
			.Where(m => m.CrewProcessing.EntryDate.Date >= (startDate)
			            && m.CrewProcessing.EntryDate.Date <= (endDate))
			.ToListAsync();
		
		var vesselsDocked = await dbContext.VesselsDocked
			.AsNoTracking()
			.Include(e=>e.CrewProcessing)
			.Include(v=>v.VesselType)
			.Where(m => m.CrewProcessing.EntryDate.Date >= (startDate)
			            && m.CrewProcessing.EntryDate.Date <= (endDate))
			.ToListAsync();
		
		var ashorePasses = await dbContext.AshorePasses
			.AsNoTracking()
			.Include(e=>e.CrewProcessing)
			.Include(v=>v.PassType)
			.Where(m => m.CrewProcessing.EntryDate.Date >= (startDate)
			            && m.CrewProcessing.EntryDate.Date <= (endDate))
			.ToListAsync();

		// Group by embarkation and disembarkation
		var groupedEmbarkations = embarkations.GroupBy(x => x.Nationality.Name);
		var groupedDisembarkations = disEmbarkations.GroupBy(x => x.Nationality.Name);
		var groupedVesselsDocked = vesselsDocked.GroupBy(x => x.VesselType.Name);
		var groupedAshorePasses = ashorePasses.GroupBy(x => x.PassType.Name);
		
		// Create new objects
		var crewProcessings = new CrewProcessingExportDto();
		var embarkationExports = new List<EmbarkationExportDto>();
		foreach (var embarkation in groupedEmbarkations)
		{
			embarkationExports.Add(new EmbarkationExportDto
			{
				Nationality = embarkation.Key,
				Number = embarkation.Sum(x => x.Count)
			});
		}
		
		var disEmbarkationExports = new List<DisEmbarkationExportDto>();
		foreach (var embarkation in groupedDisembarkations)
		{
			disEmbarkationExports.Add(new DisEmbarkationExportDto()
			{
				Nationality = embarkation.Key,
				Number = embarkation.Sum(x => x.Count)
			});
		}
		
		var ashorePassExports = new List<AshorePassExportDto>();
		foreach (var ashorePass in groupedAshorePasses)
		{
			ashorePassExports.Add(new AshorePassExportDto()
			{
				AshorePass = ashorePass.Key,
				Number = ashorePass.Sum(x => x.NumberOfPass)
			});
		}
		
		var vesselDockedExports = new List<VesselDockedExportDto>();
		foreach (var vessel in groupedVesselsDocked)
		{
			vesselDockedExports.Add(new VesselDockedExportDto()
			{
				Vessel = vessel.Key,
				Number = vessel.Sum(x => x.Count)
			});
		}
		
		crewProcessings.Embarkations = embarkationExports;
		crewProcessings.Disembarkations = disEmbarkationExports;
		crewProcessings.AshorePasses = ashorePassExports;
		crewProcessings.VesselsDocked = vesselDockedExports;
		
		return crewProcessings;
	}
	
	public async ValueTask DeleteEmbarkationDetail(Guid embarkationId, Guid crewProcessingId, CancellationToken cancellationToken)
	{
		await using var dbContext = await DbContextFactory.CreateDbContextAsync(cancellationToken);
		var query = await dbContext.Set<Embarkation>()
			.FirstOrDefaultAsync(
				x=>x.CrewProcessingId == crewProcessingId
				   && x.EmbarkationId == embarkationId, cancellationToken);
	
		dbContext.Embarkations.Remove(query!);
		await dbContext.SaveChangesAsync(cancellationToken);
	}
	
	public async ValueTask DeleteDisEmbarkationDetail(Guid disembarkationId, Guid crewProcessingId, CancellationToken cancellationToken)
	{
		await using var dbContext = await DbContextFactory.CreateDbContextAsync(cancellationToken);
		var query = await dbContext.Set<DisEmbarkation>()
			.FirstOrDefaultAsync(
				x=>x.CrewProcessingId == crewProcessingId
				   && x.DisEmbarkationId == disembarkationId, cancellationToken);
	
		dbContext.DisEmbarkations.Remove(query!);
		await dbContext.SaveChangesAsync(cancellationToken);
	}
	
	public async ValueTask DeleteVesselDockedDetail(Guid vesselDockedId, Guid crewProcessingId, CancellationToken cancellationToken)
	{
		await using var dbContext = await DbContextFactory.CreateDbContextAsync(cancellationToken);
		var query = await dbContext.Set<VesselsDocked>()
			.FirstOrDefaultAsync(
				x=>x.CrewProcessingId == crewProcessingId
				   && x.VesselDockedId == vesselDockedId, cancellationToken);
	
		dbContext.VesselsDocked.Remove(query!);
		await dbContext.SaveChangesAsync(cancellationToken);
	}
}
