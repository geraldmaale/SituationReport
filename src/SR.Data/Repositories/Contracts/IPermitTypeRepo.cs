using GreatIdeas.Repository;
using SR.Shared.DTOs.Permits;
using SR.Shared.Entities;

namespace SR.Data.Repositories.Contracts;
public interface IPermitTypeRepo: IRepositoryFactory<ApplicationDbContext, PermitType, PermitTypeDto>
{
}
