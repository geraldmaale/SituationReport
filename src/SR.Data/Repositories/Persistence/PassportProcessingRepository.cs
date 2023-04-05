using GreatIdeas.Repository;
using Microsoft.EntityFrameworkCore;
using SR.Data.Repositories.Contracts;
using SR.Shared.DTOs.Passports;
using SR.Shared.Entities;

namespace SR.Data.Repositories.Persistence;
public class PassportProcessingRepository: 
	RepositoryFactory<ApplicationDbContext, PassportProcessing, PassportProcessingDto>, IPassportProcessingRepository
{
	public PassportProcessingRepository(IDbContextFactory<ApplicationDbContext> dbContextFactory) : base(dbContextFactory)
	{
	}
	
	public async ValueTask<PassportProcessing> GetById(Guid id, CancellationToken cancellationToken)
	{
		await using var dbContext = await DbContextFactory.CreateDbContextAsync(cancellationToken);
		var query = dbContext.Set<PassportProcessing>()
			.Include(e => e.Officer)
			.AsQueryable();

		var result = await query.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

		return result!;
	}
}
