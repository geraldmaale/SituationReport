using GreatIdeas.Repository;
using Microsoft.EntityFrameworkCore;
using SR.Data.Repositories.Contracts;
using SR.Shared.Entities;

namespace SR.Data.Repositories.Persistence;
public class AshorePassTypeRepository : RepositoryFactory<ApplicationDbContext, AshorePassType>, IAshorePassTypeRepository
{
	public AshorePassTypeRepository(IDbContextFactory<ApplicationDbContext> dbContextFactory) : base(dbContextFactory)
	{
	}
}
