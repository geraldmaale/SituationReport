using GreatIdeas.Extensions.Paging;
using GreatIdeas.Repository.Paging;
using Microsoft.AspNetCore.Identity;
using SR.Shared.DTOs.User;
using SR.Shared.Identity;

namespace SR.Data.Repositories.Contracts;

public interface IUserRepository
{
	Task<PagedList<UserDto>> GetPagedUsers(PagingParams pagingParameters,
		CancellationToken cancellationToken = default);

	Task<IEnumerable<UserDto>> GetAllUsers(PagingParams pagingParameters);
	Task<UserDto?> GetUserByName(string email);
	Task<ApplicationUser?> FindById(string id);
	Task<UserDto?> GetUserById(string id);
	Task<List<IdentityUserClaim<string>>> GetClaimsAsync(string userId);
	Task RemoveClaimAsync(IdentityUserClaim<string> userClaim);
	Task AddClaimAsync(IdentityUserClaim<string> userClaim);
	ValueTask<ApplicationUser> CreateUserAsync(UserCreationDto userCreation);
	ValueTask<bool> UpdateUserAsync(string userId, UserUpdateDto userUpdate);
	Task<List<RoleDto>> GetRoles();
	ValueTask<List<UserRoleSectionDto>> GetRolesByUserAsync(string userId);
	ValueTask UpdateUserRolesAsync(string userId, List<UserRoleSectionDto> userRoles);
	ValueTask AddRoleToUserAsync(UserRoleManipulationDto userRole);
	ValueTask RemoveUserRoleAsync(UserRoleManipulationDto userRole);
	ValueTask DeleteUserById(string userId);
}