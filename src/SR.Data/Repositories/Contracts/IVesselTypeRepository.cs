using GreatIdeas.Repository;
using SR.Shared.Entities;

namespace SR.Data.Repositories.Contracts;
public interface IVesselTypeRepository: IRepositoryFactory<ApplicationDbContext, VesselType>
{

}
