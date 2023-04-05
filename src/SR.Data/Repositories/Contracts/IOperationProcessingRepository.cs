using GreatIdeas.Repository;
using SR.Shared.DTOs.Operations;
using SR.Shared.Entities;

namespace SR.Data.Repositories.Contracts;
public interface IOperationProcessingRepository: IRepositoryFactory<ApplicationDbContext, OperationInspection, OperationInspectionDto>
{
	ValueTask<IEnumerable<OperationInspectionDto>> GetAllAsync(Guid operationOfficeId,
		CancellationToken cancellationToken);

	ValueTask<OperationInspection> GetById(Guid operationOfficeId, Guid id, CancellationToken cancellationToken);
	ValueTask DeleteExistingRecords(OperationInspection model, CancellationToken cancellationToken);
}
