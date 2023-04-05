using GreatIdeas.Repository;
using Microsoft.EntityFrameworkCore;
using SR.Data.Repositories.Contracts;
using SR.Shared.DTOs.Permits;
using SR.Shared.Entities;

namespace SR.Data.Repositories.Persistence;
public class PermitProcessingDetailRepository: RepositoryFactory<ApplicationDbContext, PermitProcessingDetail, PermitProcessingDetailDto>,
	IPermitProcessingDetailRepository
{
	public PermitProcessingDetailRepository(IDbContextFactory<ApplicationDbContext> dbContextFactory) : base(dbContextFactory)
	{
	}
}
