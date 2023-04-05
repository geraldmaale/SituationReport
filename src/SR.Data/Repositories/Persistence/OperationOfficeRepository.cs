using GreatIdeas.Repository;
using Microsoft.EntityFrameworkCore;
using SR.Data.Repositories.Contracts;
using SR.Shared.DTOs.Operations;
using SR.Shared.Entities;

namespace SR.Data.Repositories.Persistence;
public class OperationOfficeRepository: RepositoryFactory<ApplicationDbContext, OperationOffice, OperationOfficeDto>, IOperationOfficeRepository
{
	public OperationOfficeRepository(IDbContextFactory<ApplicationDbContext> dbContextFactory) : base(dbContextFactory)
	{
	}
}
