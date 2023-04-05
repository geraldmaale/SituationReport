using GreatIdeas.Repository;
using SR.Shared.DTOs.Permits;
using SR.Shared.Entities;

namespace SR.Data.Repositories.Contracts;
public interface IPermitProcessingDetailRepository: IRepositoryFactory<ApplicationDbContext, PermitProcessingDetail, PermitProcessingDetailDto>
{

}
