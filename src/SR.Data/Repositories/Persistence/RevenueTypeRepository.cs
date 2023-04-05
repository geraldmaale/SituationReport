using GreatIdeas.Repository;
using Microsoft.EntityFrameworkCore;
using SR.Data.Repositories.Contracts;
using SR.Shared.Entities;

namespace SR.Data.Repositories.Persistence;
public class RevenueTypeRepository : RepositoryFactory<ApplicationDbContext, RevenueType>, IRevenueTypeRepository
{
	public RevenueTypeRepository(IDbContextFactory<ApplicationDbContext> dbContextFactory) : base(dbContextFactory)
	{
	}
}
