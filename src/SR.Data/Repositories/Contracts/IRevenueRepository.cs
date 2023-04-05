using GreatIdeas.Repository;
using SR.Shared.DTOs.Revenues;
using SR.Shared.Entities;
using SR.Shared.Params;

namespace SR.Data.Repositories.Contracts;
public interface IRevenueRepository : IRepositoryFactory<ApplicationDbContext, RevenueCollection, RevenueCollectionDto>
{
	ValueTask<RevenueCollection> GetById(Guid id, CancellationToken cancellationToken);
	ValueTask<List<RevenueCollectionDetailExportDto>> GetRevenueCollectionDetails(PagingParameters pagingParams);
	ValueTask DeleteRevenueCollectionDetail(Guid revenueCollectionDetailId, Guid revenueCollectionId,
		CancellationToken cancellationToken);
	ValueTask<MemoryStream> Export(PagingParameters pagingParams);
}
