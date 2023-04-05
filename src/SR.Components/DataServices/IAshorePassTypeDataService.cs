using GreatIdeas.Extensions;
using SR.Shared.Entities;

namespace SR.Components.DataServices;

public interface IAshorePassTypeDataService
{
	ValueTask<ApiResults<AshorePassType>> GetAllAsync(CancellationToken cancellationToken);
	ValueTask<ApiResult<AshorePassType>> GetByIdAsync(int id, CancellationToken cancellationToken);

	ValueTask<ApiResult> UpdateAsync(int id, AshorePassType model,
		CancellationToken cancellationToken);

	ValueTask<ApiResult> CreateAsync(AshorePassType model,
		CancellationToken cancellationToken);

	ValueTask<ApiResult> DeleteAsync(int id, CancellationToken cancellationToken);
}