using GreatIdeas.Repository;
using Microsoft.EntityFrameworkCore;
using SR.Data.Repositories.Contracts;
using SR.Shared.Entities;

namespace SR.Data.Repositories.Persistence;
public class NationalityTypeRepository: RepositoryFactory<ApplicationDbContext, NationalityType>, INationalityTypeRepository
{
	public NationalityTypeRepository(IDbContextFactory<ApplicationDbContext> dbContextFactory) : base(dbContextFactory)
	{
	}
}
