using GreatIdeas.Extensions;
using GreatIdeas.Extensions.Paging;
using SR.Shared.DTOs.User;

namespace SR.Components.DataServices;

public interface IUserDataService
{
	Task<ApiResult> FindUserByNameAsync(string userName);
	Task<ApiResults<RoleDto>> GetRoles();

	Task<ApiResults<UserDto>> GetUsersAsync(
		PagingParams pagingParams, CancellationToken cancellationToken);

	Task<ApiResult<UserDto?>> FindUserByIdAsync(string userId);
	Task<ApiResult> Register(RegisterDto model);
	Task<ApiResult> UpdateUserAsync(string userId, UserUpdateDto userUpdate);
	Task<ApiResult> DeleteUserAsync(string userId);
	Task<ApiResult> DeleteUserByUsernameAsync(string username);
	Task<ApiResult> Login(LoginDto model);
	Task<ApiResult> ResetPassword(ResetPasswordDto resetPasswordModel);
	Task<ApiResult> ForgotPassword(ForgotPasswordDto model);
	Task<ApiResult> ResendEmailConfirmation(ResendEmailDto model);
	Task<ApiResult> ChangeUserPasswordAsync(string userId, ChangePasswordDto model);
	Task<ApiResults<UserRoleSectionDto>> GetUserRolesAsync(string userId);
	Task<ApiResult> UpdateUserRolesAsync(string userid, List<UserRoleSectionDto> userRoles);
	Task<ApiResult> AddRoleToUserAsync(UserRoleManipulationDto userRole);
	Task<ApiResult> RemoveUserRoleAsync(UserRoleManipulationDto userRole);
}