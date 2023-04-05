using GreatIdeas.Repository;
using Mapster;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using SR.Data.Repositories.Contracts;
using SR.Shared;
using SR.Shared.DTOs.Officers;
using SR.Shared.DTOs.User;
using SR.Shared.Entities;

namespace SR.Data.Repositories.Persistence;
public class OfficerRepository: RepositoryFactory<ApplicationDbContext, Officer, OfficerDto>, IOfficerRepository
{
	private readonly IUserRepository _userRepository;
	private IMapper _mapper = new Mapper();
	
	public OfficerRepository(IDbContextFactory<ApplicationDbContext> dbContextFactory, IUserRepository userRepository)
		: base(dbContextFactory)
	{
		_userRepository = userRepository;
	}
	
	// Create Officer
	public async ValueTask<OfficerDto> CreateOfficer(OfficerCreationDto model, CancellationToken cancellationToken)
	{
		var context = await DbContextFactory.CreateDbContextAsync(cancellationToken);
		
		// Verify if the user exists
		var userExist = await _userRepository.GetUserByName(model.Username);
		if (userExist != null)
		{
			throw new ApplicationException($"Username already exists");
		}

		// Save the officer
		var entity = _mapper.Map<Officer>(model);
		var newOfficer = context.Officers.Add(entity);
		await context.SaveChangesAsync(cancellationToken);
		
		// Create the user
		var userModel = new UserCreationDto()
		{
			FullName = model.FullName,
			UserName = model.Username,
			Password = model.Password,
			ConfirmPassword = model.Password,
			Email = model.Username,
			PhoneNumber = model.PhoneNumber,
			OfficerId = newOfficer.Entity.OfficerId
		};
		_ = await _userRepository.CreateUserAsync(userModel);
		
		// Return the officer
		return _mapper.Map<OfficerDto>(entity);
	}

	// Delete Officer
	public async ValueTask<bool> DeleteOfficer(Guid officerId, CancellationToken cancellationToken)
	{
		await using var context = await DbContextFactory.CreateDbContextAsync(cancellationToken);
		await using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);

		try
		{
			var officer = await context.Officers.FirstOrDefaultAsync(o => o.OfficerId == officerId, cancellationToken);
			if (officer == null)
			{
				throw new ArgumentException("Officer does not exist");
			}
		
			// Validate if officer has  a user account
			var user = await context.Users.FirstOrDefaultAsync(x=>x.UserName == officer.Username, cancellationToken: cancellationToken);
			if (user != null)
			{
				// Delete user account
				context.Users.Remove(user);
				await context.SaveChangesAsync(cancellationToken);
			}
		
			// Delete officer
			context.Officers.Remove(officer);
			await context.SaveChangesAsync(cancellationToken);
			
			// Commit transaction
			await transaction.CommitAsync(cancellationToken);
			
			return true;
		}
		catch (DbUpdateException)
		{
			// Rollback transaction
			await transaction.RollbackAsync(cancellationToken);
			throw new DbUpdateException(StatusLabels.UnprocessableDeleteError);
		}
	}

	public async ValueTask<OfficerFullDto> GetById(Guid officerId, CancellationToken cancellationToken)
	{
		await using var context = await DbContextFactory.CreateDbContextAsync(cancellationToken);
		var query = await context.Officers
			.Include(x=>x.User)
			.ProjectToType<OfficerFullDto>()
			.FirstOrDefaultAsync(o => o.OfficerId == officerId, cancellationToken);
		
		return query!;
	}
}
