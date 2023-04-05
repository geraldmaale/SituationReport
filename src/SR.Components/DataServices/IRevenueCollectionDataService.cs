using GreatIdeas.Extensions;
using SR.Shared.DTOs.Revenues;
using SR.Shared.Entities;

namespace SR.Components.DataServices;

public interface IRevenueCollectionDataService
{
	ValueTask<ApiResults<RevenueCollectionDto>> GetAllAsync(CancellationToken cancellationToken);
	ValueTask<ApiResult<RevenueCollection>> GetByIdAsync(Guid id, CancellationToken cancellationToken);

	ValueTask<ApiResult> UpdateAsync(Guid id, RevenueCollectionManipulationDto model,
		CancellationToken cancellationToken);

	ValueTask<ApiResult> CreateAsync(RevenueCollectionManipulationDto model,
		CancellationToken cancellationToken);

	ValueTask<ApiResult> DeleteAsync(Guid id, CancellationToken cancellationToken);
	ValueTask<ApiResult> DeleteDetailAsync(Guid revenueCollectionDetailId, Guid revenueCollectionId, CancellationToken cancellationToken);
}