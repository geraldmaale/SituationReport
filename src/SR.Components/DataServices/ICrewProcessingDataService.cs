using GreatIdeas.Extensions;
using SR.Shared.DTOs.Crews;
using SR.Shared.Entities;

namespace SR.Components.DataServices;

public interface ICrewProcessingDataService
{
	ValueTask<ApiResults<CrewProcessingDto>> GetAllAsync(CancellationToken cancellationToken);
	ValueTask<ApiResult<CrewProcessing>> GetByIdAsync(Guid id, CancellationToken cancellationToken);

	ValueTask<ApiResult> UpdateAsync(Guid id, CrewProcessingManipulationDto model,
		CancellationToken cancellationToken);

	ValueTask<ApiResult> CreateAsync(CrewProcessingManipulationDto model,
		CancellationToken cancellationToken);

	ValueTask<ApiResult> DeleteAsync(Guid id, CancellationToken cancellationToken);

	ValueTask<ApiResult> DeleteEmbarkationAsync(Guid embarkationId, Guid crewProcessingId,
		CancellationToken cancellationToken);

	ValueTask<ApiResult> DeleteDisEmbarkationAsync(Guid disembarkationId, Guid crewProcessingId,
		CancellationToken cancellationToken);

	ValueTask<ApiResult> DeleteVesselDockedAsync(Guid vesselDockedId, Guid crewProcessingId,
		CancellationToken cancellationToken);
}