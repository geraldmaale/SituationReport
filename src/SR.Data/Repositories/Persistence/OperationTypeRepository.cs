using GreatIdeas.Repository;
using Microsoft.EntityFrameworkCore;
using SR.Data.Repositories.Contracts;
using SR.Shared.Entities;

namespace SR.Data.Repositories.Persistence;
public class OperationTypeRepository: RepositoryFactory<ApplicationDbContext, OperationType>, IOperationTypeRepository
{
	public OperationTypeRepository(IDbContextFactory<ApplicationDbContext> dbContextFactory) : base(dbContextFactory)
	{
	}
}
