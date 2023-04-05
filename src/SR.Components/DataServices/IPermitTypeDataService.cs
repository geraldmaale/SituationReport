using GreatIdeas.Extensions;
using SR.Shared.DTOs.Permits;

namespace SR.Components.DataServices;

public interface IPermitTypeDataService
{
	ValueTask<ApiResults<PermitTypeDto>> GetAllAsync(CancellationToken cancellationToken);
	ValueTask<ApiResult<PermitTypeDto>> GetByIdAsync(int id, CancellationToken cancellationToken);

	ValueTask<ApiResult> UpdateAsync(int id, PermitTypeManipulationDto model,
		CancellationToken cancellationToken);

	ValueTask<ApiResult> CreateAsync(PermitTypeManipulationDto model,
		CancellationToken cancellationToken);

	ValueTask<ApiResult> DeleteAsync(int id, CancellationToken cancellationToken);
}