using GreatIdeas.Extensions;
using SR.Shared.DTOs.Operations;
using SR.Shared.Entities;

namespace SR.Components.DataServices;

public interface IOperationOfficeDataService
{
	ValueTask<ApiResults<OperationOfficeDto>> GetAllAsync(CancellationToken cancellationToken);
	ValueTask<ApiResult<OperationOffice>> GetByIdAsync(Guid id, CancellationToken cancellationToken);
	ValueTask<ApiResult<OperationOfficeDto>> GetByIdDtoAsync(Guid id, CancellationToken cancellationToken);

	ValueTask<ApiResult> UpdateAsync(Guid id, OperationOffice model,
		CancellationToken cancellationToken);

	ValueTask<ApiResult> CreateAsync(OperationOffice model,
		CancellationToken cancellationToken);

	ValueTask<ApiResult> DeleteAsync(Guid id, CancellationToken cancellationToken);
}