using GreatIdeas.Extensions;
using SR.Shared.DTOs.Permits;
using SR.Shared.Entities;

namespace SR.Components.DataServices;

public interface IPermitProcessingDataService
{
	ValueTask<ApiResults<PermitProcessingDto>> GetAllAsync(CancellationToken cancellationToken);
	ValueTask<ApiResult<PermitProcessing>> GetByIdAsync(Guid id, CancellationToken cancellationToken);

	ValueTask<ApiResult> UpdateAsync(Guid id, PermitProcessingManipulationDto model,
		CancellationToken cancellationToken);

	ValueTask<ApiResult> CreateAsync(PermitProcessingManipulationDto model,
		CancellationToken cancellationToken);

	ValueTask<ApiResult> DeleteAsync(Guid id, CancellationToken cancellationToken);
	ValueTask<ApiResult> DeleteDetailAsync(Guid permitProcessingDetailId, Guid permitProcessingId, CancellationToken cancellationToken);
}