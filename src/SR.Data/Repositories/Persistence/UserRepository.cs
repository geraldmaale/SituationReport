using System.Security.Claims;
using GreatIdeas.Extensions;
using GreatIdeas.Extensions.Paging;
using GreatIdeas.Repository;
using GreatIdeas.Repository.Paging;
using IdentityModel;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;
using SR.Data.Repositories.Contracts;
using SR.Shared;
using SR.Shared.DTOs.User;
using SR.Shared.Identity;

namespace SR.Data.Repositories.Persistence;

public class UserRepository : RepositoryFactory<ApplicationDbContext, ApplicationUser, UserDto>, IUserRepository
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    
    public UserRepository(IDbContextFactory<ApplicationDbContext> dbContextFactory, 
        UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager) : base(dbContextFactory)
    {
	    _userManager = userManager;
	    _roleManager = roleManager;
    }

    public async Task<PagedList<UserDto>> GetPagedUsers(PagingParams pagingParameters,
            CancellationToken cancellationToken = default)
    {
	    var users = _userManager.Users.ProjectToType<UserDto>().AsQueryable();

        var paged = await PagedList<UserDto>.ToPagedListAsync(users.ProjectToType<UserDto>(),
            pagingParameters.PageIndex, pagingParameters.PageSize, cancellationToken);

        return paged;
    }

    public async Task<IEnumerable<UserDto>> GetAllUsers(PagingParams pagingParameters)
    {
        var users = await _userManager.Users
	        .AsNoTracking()
	        .ProjectToType<UserDto>().ToListAsync();
        
        return users;
    }

    public async Task<UserDto?> GetUserByName(string email)
    {
	    var user = await _userManager.Users
		    .AsNoTracking()
		    .ProjectToType<UserDto>()
		    .FirstOrDefaultAsync(x => x.Email == email);

        return user;
    }

    public async Task<ApplicationUser?> FindById(string id)
    {
        var user = await _userManager.Users
            .FirstOrDefaultAsync(x => x.Id == id);

        return user;
    }

    public async Task<UserDto?> GetUserById(string id)
    {
        var user =await _userManager.Users.ProjectToType<UserDto>().FirstOrDefaultAsync(x => x.Id == id);

        return user;
    }

    public async Task<List<IdentityUserClaim<string>>> GetClaimsAsync(string userId)
    {
        var claims = await DbContext.UserClaims
            .AsNoTracking()
            .Where(x => x.UserId == userId).ToListAsync();

        return claims;
    }

    public async Task RemoveClaimAsync(IdentityUserClaim<string> userClaim)
    {
        var claims = DbContext.UserClaims
            .Remove(userClaim);
        await DbContext.SaveChangesAsync();
    }

    public async Task AddClaimAsync(IdentityUserClaim<string> userClaim)
    {
        var claims = DbContext.UserClaims
            .Add(userClaim);

        await DbContext.SaveChangesAsync();
    }
    
    public async ValueTask<ApplicationUser> CreateUserAsync(UserCreationDto userCreation)
    {
	    var user = await CreateUser(userCreation);
	    return user;
    }
    
    private async Task<ApplicationUser> CreateUser(UserCreationDto userCreation)
    {
        var user = new ApplicationUser()
        {
            UserName = userCreation.UserName,
            Email = userCreation.Email,
            EmailConfirmed = true,
            PhoneNumber = userCreation.PhoneNumber,
            FullName = userCreation.FullName,
            OfficerId = userCreation.OfficerId,
        };

        var result = await _userManager.CreateAsync(user, userCreation.Password);
        if (!result.Succeeded)
        {
            throw new ApplicationException(result.Errors.FirstOrDefault()?.Description);
        }

        if (!string.IsNullOrEmpty(user.Email))
        {
            await _userManager.AddClaimsAsync(user, new Claim[]
            {
                new Claim(JwtClaimTypes.Id, $"{user.Id}"),
                new Claim(JwtClaimTypes.Email, user.Email),
                new Claim(JwtClaimTypes.PhoneNumber, user.PhoneNumber),
                new Claim(JwtClaimTypes.PreferredUserName, user.UserName),
                new Claim(JwtClaimTypes.Name, user.FullName),
                new Claim("OfficerId", user.OfficerId.ToString()),
            });
        }
        else
        {
            await _userManager.AddClaimsAsync(user, new Claim[]
            {
                new Claim(JwtClaimTypes.Id, $"{user.Id}"),
                new Claim(JwtClaimTypes.PhoneNumber, user.PhoneNumber),
                new Claim(JwtClaimTypes.PreferredUserName, user.UserName),
                new Claim(JwtClaimTypes.Name, user.FullName),
                new Claim("OfficerId", user.OfficerId.ToString()),
            });
        }

        return user;
    }
    
      public async ValueTask<bool> UpdateUserAsync(string userId, UserUpdateDto userUpdate)
    {

        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            Log.Error(StatusLabels.NotFound(userId));
            throw new ApplicationException(StatusLabels.NotFound($"{userId}"));
        }

        user.UserName = userUpdate.Email;
        if (userUpdate.Email != null) user.Email = userUpdate.Email;
        user.PhoneNumber = userUpdate.PhoneNumber;
        user.OfficerId = userUpdate.OfficerId;
        user.FullName = userUpdate.FullName;

        var result = await _userManager.UpdateAsync(user);
        if (result.Succeeded)
        {
            // update claims
            var claims = await _userManager.GetClaimsAsync(user);
            if (claims != null)
            {
                var phoneNumber = claims.FirstOrDefault(c => c.Type == JwtClaimTypes.PhoneNumber);
                var email = claims.FirstOrDefault(c => c.Type == JwtClaimTypes.Email && c.Value.Length > 0);
                var name = claims.FirstOrDefault(c => c.Type == JwtClaimTypes.Name);
                var officerId = claims.FirstOrDefault(c => c.Type == "OfficerId");

                await _userManager.ReplaceClaimAsync(user, name,
                    new Claim(JwtClaimTypes.Name, $"{user.FullName}"));

                await _userManager.ReplaceClaimAsync(user, phoneNumber,
                    new Claim(JwtClaimTypes.PhoneNumber, user.PhoneNumber));
                
                await _userManager.ReplaceClaimAsync(user, officerId,
	                new Claim("OfficerId", user.OfficerId.ToString()));

                // Check for nullable email
                if (email != null && !string.IsNullOrEmpty(user.Email))
                {
                    await _userManager.ReplaceClaimAsync(user, email,
                        new Claim(JwtClaimTypes.Email, user.Email));
                }
                else if (email == null && !string.IsNullOrEmpty(user.Email))
                {
                    await _userManager.AddClaimsAsync(user, new Claim[]
                    {
                            new Claim(JwtClaimTypes.Email, $"{user.Email}"),
                    });
                }
                else if (email != null && string.IsNullOrEmpty(user.Email))
                {
                    await _userManager.RemoveClaimAsync(user, email);
                }
            }
            return true;
        }

        return false;
    }
    
    public async Task<List<RoleDto>> GetRoles()
    {
        return await _roleManager.Roles
            .AsNoTracking()
            .ProjectToType<RoleDto>()
            .OrderBy(r => r.Name)
            .ToListAsync();
    }
    
    public async ValueTask<List<UserRoleSectionDto>> GetRolesByUserAsync(string userId)
    {
	    var user = await GetUserById(userId);
	    
	    var allRoles = _roleManager.Roles.ToList();

	    List<UserRoleSectionDto> userRoleSections = new();

	    foreach (var role in allRoles)
	    {
		    var userRole = new UserRoleSectionDto
		    {
			    Id = role.Id,
			    RoleName = role.Name
		    };

		    if (await _userManager.IsInRoleAsync(user.Adapt<ApplicationUser>(), role.Name))
		    {
			    userRole.IsSelected = true;
		    }
		    else
		    {
			    userRole.IsSelected = false;
		    }

		    userRoleSections.Add(userRole);
	    }

	    return userRoleSections.OrderBy(x => x.RoleName).ToList();
    }

    public async ValueTask UpdateUserRolesAsync(string userId, List<UserRoleSectionDto> userRoles)
    {
	    var user = await _userManager.FindByIdAsync(userId);

	    for (int userRole = 0; userRole < userRoles.Count; userRole++)
	    {
		    IdentityResult result = null;

		    if (userRoles[userRole].IsSelected &&
		        !(await _userManager.IsInRoleAsync(user, userRoles[userRole].RoleName)))
		    {
			    result = await _userManager.AddToRoleAsync(user, userRoles[userRole].RoleName);
		    }
		    else if (!userRoles[userRole].IsSelected &&
		             (await _userManager.IsInRoleAsync(user, userRoles[userRole].RoleName)))
		    {
			    result = await _userManager.RemoveFromRoleAsync(user, userRoles[userRole].RoleName);
		    }
		    else
		    {
			    continue;
		    }

		    if (userRole < (userRoles.Count - 1))
		    {
			    continue;
		    }
	    }
    }

    public async ValueTask AddRoleToUserAsync(UserRoleManipulationDto userRole)
    {
	    var user = await _userManager.FindByIdAsync(userRole.UserId);

	    var result = await _userManager.AddToRoleAsync(user, userRole.RoleName);
	    if (!result.Succeeded)
	    {
		    throw new ApplicationException(result.Errors.First().Description);
	    }
    }

    public async ValueTask RemoveUserRoleAsync(UserRoleManipulationDto userRole)
    {
	    var user = await _userManager.FindByIdAsync(userRole.UserId);

	    var result = await _userManager.RemoveFromRoleAsync(user, userRole.RoleName);
	    if (!result.Succeeded)
	    {
		    throw new ApplicationException(result.Errors.First().Description);
	    }
    }
    
    public async ValueTask DeleteUserById(string userId)
    {
	    var user = await _userManager.FindByIdAsync(userId);
	     
	    try
	    {
		    var sysAdmins = await _userManager.GetUsersInRoleAsync(UserRoles.Administrator);
		    var userInSysAdmin = await _userManager.IsInRoleAsync(user, UserRoles.Administrator);
		    if (sysAdmins.Count < 2 && userInSysAdmin)
		    {
			    throw new ApplicationException("You can delete Administrator if they are more than one");
		    }

		    await _userManager.DeleteAsync(user);
			Log.Information("User deleted successfully");
	    }
	    catch (DbUpdateException)
	    {
		    Log.Error("Could not delete User: {UserName}. User is still in use", user.UserName);
		    throw new ApplicationException($"Could not delete User: {user.UserName}. User is still in use.");
	    }
	    catch (Exception e)
	    {
		    Log.Fatal(e, StatusLabels.UnprocessableDeleteError);
		    throw new Exception(e.Message);
	    }
    }
}
