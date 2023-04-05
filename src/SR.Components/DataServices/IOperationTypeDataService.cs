using GreatIdeas.Extensions;
using SR.Shared.Entities;

namespace SR.Components.DataServices;

public interface IOperationTypeDataService
{
	ValueTask<ApiResults<OperationType>> GetAllAsync(CancellationToken cancellationToken);
	ValueTask<ApiResult<OperationType>> GetByIdAsync(int id, CancellationToken cancellationToken);

	ValueTask<ApiResult> UpdateAsync(int id, OperationType model,
		CancellationToken cancellationToken);

	ValueTask<ApiResult> CreateAsync(OperationType model,
		CancellationToken cancellationToken);

	ValueTask<ApiResult> DeleteAsync(int id, CancellationToken cancellationToken);
}