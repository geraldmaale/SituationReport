using GreatIdeas.Extensions;
using SR.Shared.Entities;

namespace SR.Components.DataServices;

public interface IVesselTypeDataService
{
	ValueTask<ApiResults<VesselType>> GetAllAsync(CancellationToken cancellationToken);
	ValueTask<ApiResult<VesselType>> GetByIdAsync(int id, CancellationToken cancellationToken);

	ValueTask<ApiResult> UpdateAsync(int id, VesselType model,
		CancellationToken cancellationToken);

	ValueTask<ApiResult> CreateAsync(VesselType model,
		CancellationToken cancellationToken);

	ValueTask<ApiResult> DeleteAsync(int id, CancellationToken cancellationToken);
}