using GreatIdeas.Repository;
using Microsoft.EntityFrameworkCore;
using SR.Data.Repositories.Contracts;
using SR.Shared.DTOs.Permits;
using SR.Shared.Entities;

namespace SR.Data.Repositories.Persistence;
public class PermitProcessingRepository: RepositoryFactory<ApplicationDbContext, PermitProcessing, PermitProcessingDto>,IPermitProcessingRepository
{
	public PermitProcessingRepository(IDbContextFactory<ApplicationDbContext> dbContextFactory) : base(dbContextFactory)
	{
	}
	
	public async ValueTask<PermitProcessing> GetById(Guid id, CancellationToken cancellationToken)
	{
		await using var dbContext = await DbContextFactory.CreateDbContextAsync(cancellationToken);
		var query = dbContext.Set<PermitProcessing>()
			.Include(e => e.PermitProcessingDetails)
			.ThenInclude(e => e.Nationality)
			.Include(e=>e.PermitProcessingDetails).ThenInclude(x=>x.PermitType)
			.Include(e => e.Officer)
			.AsQueryable();
		
		var result = await query.FirstOrDefaultAsync(x=>x.Id == id, cancellationToken);
		
		return result!;
	}
	
	public async ValueTask DeletePermitDetail(Guid permitProcessingDetailId, Guid permitProcessingId, CancellationToken cancellationToken)
	{
		await using var dbContext = await DbContextFactory.CreateDbContextAsync(cancellationToken);
		var query = await dbContext.Set<PermitProcessingDetail>()
			.FirstOrDefaultAsync(
				x=>x.PermitProcessingDetailId == permitProcessingDetailId
				   && x.PermitProcessingId == permitProcessingId, cancellationToken);
	
		dbContext.PermitProcessingDetails.Remove(query!);
		await dbContext.SaveChangesAsync(cancellationToken);
	}
}
