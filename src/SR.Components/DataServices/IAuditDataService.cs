using GreatIdeas.Extensions;
using GreatIdeas.Extensions.Paging;
using Radzen;
using SR.Shared.DTOs.Audits;
using SR.Shared.Params;

namespace SR.Components.DataServices;

public interface IAuditDataService
{
	ValueTask<ApiResults<AuditDto>> GetAllAsync(CancellationToken cancellationToken);
	Task<PagingResponse<AuditDto>> GetPagedAsync(
		AuditPagingParameters pagingParameters, CancellationToken token);

	Task<PagingResponse<AuditDto>> GetPagedAsync(
		LoadDataArgs args, CancellationToken token);
	ValueTask<ApiResult<AuditDto>> GetByIdAsync(Guid id, CancellationToken cancellationToken);
	Task<ApiResult> ExportAsync(AuditPagingParameters pagingParams, CancellationToken token);
}