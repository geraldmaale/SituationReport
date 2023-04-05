using GreatIdeas.Extensions;
using SR.Shared.Entities;

namespace SR.Components.DataServices;

public interface IRevenueTypeDataService
{
	ValueTask<ApiResults<RevenueType>> GetAllAsync(CancellationToken cancellationToken);
	ValueTask<ApiResult<RevenueType>> GetByIdAsync(int id, CancellationToken cancellationToken);

	ValueTask<ApiResult> UpdateAsync(int id, RevenueType model,
		CancellationToken cancellationToken);

	ValueTask<ApiResult> CreateAsync(RevenueType model,
		CancellationToken cancellationToken);

	ValueTask<ApiResult> DeleteAsync(int id, CancellationToken cancellationToken);
}