using GreatIdeas.Repository;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using SR.Data.Repositories.Contracts;
using SR.Shared.DTOs.Permits;
using SR.Shared.Entities;

namespace SR.Data.Repositories.Persistence;
public class PermitTypeRepo: RepositoryFactory<ApplicationDbContext, PermitType, PermitTypeDto>, IPermitTypeRepo
{
	private IMapper _mapper = new Mapper();
	
	
	public PermitTypeRepo(IDbContextFactory<ApplicationDbContext> dbContextFactory) : base(dbContextFactory)
	{
	}

}
