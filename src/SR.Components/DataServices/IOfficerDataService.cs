using GreatIdeas.Extensions;
using SR.Shared.DTOs.Officers;
using SR.Shared.Entities;

namespace SR.Components.DataServices;

public interface IOfficerDataService
{
	ValueTask<ApiResults<OfficerDto>> GetAllAsync(CancellationToken cancellationToken);
	ValueTask<ApiResult<OfficerFullDto>> GetByIdAsync(Guid id, CancellationToken cancellationToken);

	ValueTask<ApiResult> UpdateAsync(Guid id, OfficerUpdateDto model,
		CancellationToken cancellationToken);

	ValueTask<ApiResult<OfficerDto>> CreateAsync(OfficerCreationDto model,
		CancellationToken cancellationToken);

	ValueTask<ApiResult> DeleteAsync(Guid id, CancellationToken cancellationToken);
}