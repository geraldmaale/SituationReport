using GreatIdeas.Repository;
using GreatIdeas.Repository.Paging;
using SR.Shared.DTOs;
using SR.Shared.DTOs.Audits;
using SR.Shared.Entities;
using SR.Shared.Params;

namespace SR.Data.Repositories.Contracts;
public interface IAuditRepository: IRepositoryFactory<ApplicationDbContext, Audit, AuditDto>
{
	Task<PagedList<AuditDto>> GetPagedAuditsAsync(AuditPagingParameters pagingPagingParameters,
		CancellationToken cancellationToken);

	Task<MemoryStream> Export(AuditPagingParameters pagingParameters, CancellationToken cancellationToken);
}
