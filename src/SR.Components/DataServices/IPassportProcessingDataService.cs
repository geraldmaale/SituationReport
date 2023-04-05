using GreatIdeas.Extensions;
using SR.Shared.DTOs.Passports;
using SR.Shared.Entities;

namespace SR.Components.DataServices;

public interface IPassportProcessingDataService
{
	ValueTask<ApiResults<PassportProcessingDto>> GetAllAsync(CancellationToken cancellationToken);
	ValueTask<ApiResult<PassportProcessing>> GetByIdAsync(Guid id, CancellationToken cancellationToken);

	ValueTask<ApiResult> UpdateAsync(Guid id, PassportProcessingManipulationDto model,
		CancellationToken cancellationToken);

	ValueTask<ApiResult> CreateAsync(PassportProcessingManipulationDto model,
		CancellationToken cancellationToken);

	ValueTask<ApiResult> DeleteAsync(Guid id, CancellationToken cancellationToken);
}