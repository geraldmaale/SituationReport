using GreatIdeas.Repository;
using SR.Shared.DTOs.Operations;
using SR.Shared.Entities;

namespace SR.Data.Repositories.Contracts;
public interface IOperationOfficeRepository: IRepositoryFactory<ApplicationDbContext, OperationOffice, OperationOfficeDto>
{

}
