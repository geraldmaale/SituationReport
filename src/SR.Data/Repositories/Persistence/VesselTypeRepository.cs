using GreatIdeas.Repository;
using Microsoft.EntityFrameworkCore;
using SR.Data.Repositories.Contracts;
using SR.Shared.Entities;

namespace SR.Data.Repositories.Persistence;
public class VesselTypeRepository: RepositoryFactory<ApplicationDbContext, VesselType>, IVesselTypeRepository
{
	public VesselTypeRepository(IDbContextFactory<ApplicationDbContext> dbContextFactory) : base(dbContextFactory)
	{
	}
}
