using GreatIdeas.Extensions;
using SR.Shared.Params;

namespace SR.Components.DataServices;
public interface IReportsDataService
{
	Task<ApiResult> ExportRevenues(PagingParameters pagingParams, CancellationToken cancellationToken);
}