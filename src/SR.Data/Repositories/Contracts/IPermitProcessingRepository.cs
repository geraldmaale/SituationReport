using GreatIdeas.Repository;
using SR.Shared.DTOs.Permits;
using SR.Shared.Entities;

namespace SR.Data.Repositories.Contracts;
public interface IPermitProcessingRepository: IRepositoryFactory<ApplicationDbContext, PermitProcessing, PermitProcessingDto>
{
	ValueTask<PermitProcessing> GetById(Guid id, CancellationToken cancellationToken);

	ValueTask DeletePermitDetail(Guid permitProcessingDetailId, Guid permitProcessingId,
		CancellationToken cancellationToken);
}
