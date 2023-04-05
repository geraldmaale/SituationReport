using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using SR.Data.Repositories.Contracts;
using SR.Shared.DTOs.Permits;
using SR.Shared.Entities;

namespace SR.Data.Repositories.Persistence;
// public class PermitTypeRepository: RepositoryFactory<ApplicationDbContext, PermitType, PermitTypeDto>, IPermitTypeRepository
// {
// 	public PermitTypeRepository(IDbContextFactory<ApplicationDbContext> dbContextFactory) : base(dbContextFactory)
// 	{
// 	}
public class PermitTypeRepository:  IPermitTypeRepository
{
	private readonly IDbContextFactory<ApplicationDbContext> _dbContextFactory;
	private DbSet<PermitType> _dbSet;
	private IMapper _mapper = new Mapper();

	public PermitTypeRepository(IDbContextFactory<ApplicationDbContext> dbContextFactory)
	{
		_dbContextFactory = dbContextFactory;
	}
	
	public async ValueTask DeleteAsync(int id, CancellationToken cancellationToken)
	{
		var dbContext = await _dbContextFactory.CreateDbContextAsync(cancellationToken);
		_dbSet = dbContext.Set<PermitType>();
		var permitType = await _dbSet.FindAsync(id);
		if (permitType == null)
		{
			throw new ArgumentNullException(nameof(permitType));
		}
		_dbSet.Remove(permitType);
		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async ValueTask<IReadOnlyList<PermitTypeDto>> GetAllAsync(CancellationToken cancellationToken)
	{
		var dbContext = await _dbContextFactory.CreateDbContextAsync(cancellationToken);
		_dbSet = dbContext.Set<PermitType>();
		var query = await _dbSet
			.AsNoTracking()
			.Select(x=> new PermitTypeDto()
			{
				Id = x.Id,
				Name = x.Name,
			})
			.ToListAsync(cancellationToken);
		return query;
	}
	
	public async ValueTask<PermitTypeDto> GetByIdAsync(int id, CancellationToken cancellationToken)
	{
		var dbContext = await _dbContextFactory.CreateDbContextAsync(cancellationToken);
		_dbSet = dbContext.Set<PermitType>();
		var query = await _dbSet
			.AsNoTracking()
			.Where(x=>x.Id == id)
			.Select(x=> new PermitTypeDto()
			{
				Id = x.Id,
				Name = x.Name,
			})
			.FirstOrDefaultAsync(cancellationToken);
		
		return query!;
	}
	
	public async ValueTask InsertAsync(PermitTypeManipulationDto permitType, CancellationToken cancellationToken)
	{
		var dbContext = await _dbContextFactory.CreateDbContextAsync(cancellationToken);
		_dbSet = dbContext.Set<PermitType>();
		var entity = new PermitType()
		{
			Name = permitType.Name,
		};
		_dbSet.Add(entity);
		await dbContext.SaveChangesAsync(cancellationToken);
	}
	
	public async ValueTask UpdateAsync(int id, PermitTypeManipulationDto permitType, CancellationToken cancellationToken)
	{
		var dbContext = await _dbContextFactory.CreateDbContextAsync(cancellationToken);
		_dbSet = dbContext.Set<PermitType>();
		var entity = await _dbSet.FindAsync(id);
		if (entity == null)
		{
			throw new ArgumentNullException(nameof(entity));
		}
		_mapper.Map(permitType, entity);
		await dbContext.SaveChangesAsync(cancellationToken);
	}
	
}
