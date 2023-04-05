using GreatIdeas.Repository;
using SR.Shared.Entities;

namespace SR.Data.Repositories.Contracts;
public interface IOperationTypeRepository: IRepositoryFactory<ApplicationDbContext, OperationType>
{

}
