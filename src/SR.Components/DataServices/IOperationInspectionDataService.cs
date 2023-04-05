using GreatIdeas.Extensions;
using SR.Shared.DTOs.Operations;
using SR.Shared.Entities;

namespace SR.Components.DataServices;

public interface IOperationInspectionDataService
{
	ValueTask<ApiResults<OperationInspectionDto>> GetAllAsync(Guid operationOfficeId,
		CancellationToken cancellationToken);
	ValueTask<ApiResult<OperationInspection>> GetByIdAsync(Guid operationOfficeId, Guid id,
		CancellationToken cancellationToken);

	ValueTask<ApiResult> UpdateAsync(Guid operationOfficeId, Guid id, OperationInspectionManipulationDto model,
		CancellationToken cancellationToken);

	ValueTask<ApiResult> CreateAsync(Guid operationOfficeId, OperationInspectionManipulationDto model,
		CancellationToken cancellationToken);

	ValueTask<ApiResult> DeleteAsync(Guid operationOfficeId, Guid id, CancellationToken cancellationToken);
}