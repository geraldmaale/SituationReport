using GreatIdeas.Extensions;
using SR.Shared.Entities;

namespace SR.Components.DataServices;

public interface INationalityTypeDataService
{
	ValueTask<ApiResults<NationalityType>> GetAllAsync(CancellationToken cancellationToken);
	ValueTask<ApiResult<NationalityType>> GetByIdAsync(int id, CancellationToken cancellationToken);

	ValueTask<ApiResult> UpdateAsync(int id, NationalityType model,
		CancellationToken cancellationToken);

	ValueTask<ApiResult> CreateAsync(NationalityType model,
		CancellationToken cancellationToken);

	ValueTask<ApiResult> DeleteAsync(int id, CancellationToken cancellationToken);
}