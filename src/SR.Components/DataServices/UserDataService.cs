using System.Security.Claims;
using System.Text;
using GreatIdeas.Extensions;
using GreatIdeas.Extensions.Paging;
using IdentityModel;
using MapsterMapper;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Serilog;
using SR.Components.Helpers;
using SR.Data.Repositories.Contracts;
using SR.Shared;
using SR.Shared.DTOs.Messaging;
using SR.Shared.DTOs.User;
using SR.Shared.Identity;

namespace SR.Components.DataServices;

public class UserDataService : IUserDataService
{
    private readonly IMapper _mapper = new Mapper();
    private readonly IUserRepository _userRepository;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly NavigationManager _navigationManager;
    private readonly IEmailSender _emailSender;

    public UserDataService(UserManager<ApplicationUser> userManager,
        NavigationManager navigationManager,
        IEmailSender emailSender, IUserRepository userRepository)
    {
        _userManager = userManager;
        _navigationManager = navigationManager;
        _emailSender = emailSender;
        _userRepository = userRepository;
    }

   public async Task<ApiResult> FindUserByNameAsync(string userName)
    {
        var user = await _userRepository.GetUserByName(userName);
        if (user is null)
        {
            Log.Warning(StatusLabels.NotFound($"{userName}"));
            return new ApiResult() { Message = StatusLabels.NotFound($"{userName}") };
        }

        return new ApiResult<UserDto>() { IsSuccessful = true, Result = user };
    }

    public async Task<ApiResults<RoleDto>> GetRoles()
    {
        var roles = await _userRepository.GetRoles();

        return new ApiResults<RoleDto>() { IsSuccessful = true, Results = roles };
    }

    public async Task<ApiResults<UserDto>> GetUsersAsync(
        PagingParams pagingParams, CancellationToken cancellationToken)
    {
        var users = await _userRepository.GetAllUsers(pagingParams);

        return new ApiResults<UserDto>
        {
            IsSuccessful = true,
            Results = _mapper.Map<UserDto[]>(users)
        };
    }

    public async Task<ApiResult<UserDto?>> FindUserByIdAsync(string userId)
    {
        var user = await _userRepository.GetUserById(userId);

        return new ApiResult<UserDto?> { IsSuccessful = true, Result = user };
    }
    
     public async Task<ApiResult> Register(RegisterDto model)
    {
        var user = new ApplicationUser
        {
            FullName = model.ContactName,
            UserName = model.Email,
            Email = model.Email,
        };

        var result = await _userManager.CreateAsync(user, model.Password);

        if (result.Succeeded)
        {
            Log.Information("User created a new account with password.");

            result = await _userManager.AddClaimsAsync(user, new Claim[] {
                new Claim(JwtClaimTypes.Name, $"{user.FullName}"),
                new Claim(JwtClaimTypes.Id, user.Id),
                new Claim(JwtClaimTypes.Email, user.Email)
            });

            // Create user role
            // _ = await _userManager.AddToRoleAsync(user, UserRoles.User);

            // Generate code for confirmation
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

            // Send confirmation email
            var queryParameters = new Dictionary<string, object>();
            queryParameters.Add("userId", user.Id);
            queryParameters.Add("code", code);

            var baseUrl = _navigationManager.BaseUri;
            var callbackUrl = _navigationManager.GetUriWithQueryParameters($"{baseUrl}identity/account/confirmemail", queryParameters);

            var message = "<h3>Welcome to the GIBS Election!</h3>" +
                          "<p>Please click on the link below to confirm your email address.</p>" +
                          "<p><a href=\"" + callbackUrl + "\">Confirm your email</a></p>" +
                          "<br>" +
                          "<h4>" + EmailDetails.TeamNameTag + "</h4>" +
                          "<h3>" + EmailDetails.BusinessNameTag + "</h3>" +
                          "<p>Email: " + EmailDetails.SenderAddress + "</p>" +
                          "<p>Website: " + EmailDetails.Website + "</p>";

            var response = await _emailSender.SendEmailAsync(
                email: model.Email,
                subject: "Registration Confirmation",
                body: message,
                name: EmailDetails.SenderName
            );


            if (_userManager.Options.SignIn.RequireConfirmedAccount)
            {
                return new ApiResult
                {
                    IsSuccessful = true,
                    Message = "Verification email sent to " + model.Email
                };
            }

        }

        return new ApiResult() { Message = result.Errors.FirstOrDefault()!.Description };
    }

    public async Task<ApiResult> UpdateUserAsync(string userId, UserUpdateDto userUpdate)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return new ApiResult() { Message = StatusLabels.NotFound($"User :{userId}") };
            }

            _mapper.Map(userUpdate, user);

            var result = await _userManager.UpdateAsync(user);

            // Update claims
            var claims = await _userManager.GetClaimsAsync(user);
            if (claims != null)
            {
                var name = claims.FirstOrDefault(c => c.Type == JwtClaimTypes.Name);

                await _userManager.ReplaceClaimAsync(user, name,
                    new Claim(JwtClaimTypes.Name, user.FullName));
            }

            Log.Information("User updated successfully.");

            return new ApiResult()
            {
                IsSuccessful = true,
                Message = StatusLabels.UpdateSuccess("User")
            };

        }
        catch (Exception e)
        {
            Log.Fatal(e.Message, StatusLabels.UnprocessableError);
            return new ApiResult() { Message = StatusLabels.UnprocessableError };
        }
    }

    public async Task<ApiResult> DeleteUserAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return new ApiResult() { Message = StatusLabels.NotFound("User") };
        }

        try
        {
            var admins = await _userManager.GetUsersInRoleAsync(UserRoles.Administrator);
            var admin = await _userManager.IsInRoleAsync(user, UserRoles.Administrator);
            if (admins.Count < 2 && admin)
            {
                return new ApiResult()
                {
                    Message = "You can disable Administrator if they are more than one"
                };
            }

            // var isSysAdmin = await _userManager.IsInRoleAsync(user, UserRoles.SystemAdmin);
            // if (isSysAdmin)
            // {
            //     return "You cannot delete the System Admin");
            // }

            await _userManager.DeleteAsync(user);
            Log.Information("User deleted successfully");
            return new ApiResult() { IsSuccessful = true, Message = StatusLabels.DeleteSuccess("User") };
        }
        catch (DbUpdateException)
        {
            Log.Error($"Could not delete User: {user.UserName}. User is still in use.");
            return new ApiResult()
            {
                Message = $"Could not delete User: {user.UserName}. User is still in use."
            };
        }
        catch (Exception e)
        {
            Log.Fatal(e.Message, StatusLabels.UnprocessableDeleteError);
            return new ApiResult() { Message = StatusLabels.UnprocessableDeleteError };
        }
    }

    public async Task<ApiResult> DeleteUserByUsernameAsync(string username)
    {
	    var user = await _userManager.FindByNameAsync(username);
	    if (user == null)
	    {
		    return new ApiResult() { Message = StatusLabels.NotFound("User") };
	    }

	    try
	    {
		    var admins = await _userManager.GetUsersInRoleAsync(UserRoles.Administrator);
		    var admin = await _userManager.IsInRoleAsync(user, UserRoles.Administrator);
		    if (admins.Count < 2 && admin)
		    {
			    return new ApiResult()
			    {
				    Message = "You can disable Administrator if they are more than one"
			    };
		    }

		    await _userManager.DeleteAsync(user);
		    Log.Information("User deleted successfully");
		    return new ApiResult() { IsSuccessful = true, Message = StatusLabels.DeleteSuccess("User") };
	    }
	    catch (DbUpdateException)
	    {
		    Log.Error($"Could not delete User: {user.UserName}. User is still in use.");
		    return new ApiResult()
		    {
			    Message = $"Could not delete User: {user.UserName}. User is still in use."
		    };
	    }
	    catch (Exception e)
	    {
		    Log.Fatal(e.Message, StatusLabels.UnprocessableDeleteError);
		    return new ApiResult() { Message = StatusLabels.UnprocessableDeleteError };
	    }
    }

    public async Task<ApiResult> Login(LoginDto model)
    {
        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user == null)
        {
            return new ApiResult { IsSuccessful = false, Message = "Invalid email attempt." };
        }

        return new ApiResult { IsSuccessful = true };
    }

    public async Task<ApiResult> ResetPassword(ResetPasswordDto resetPasswordModel)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(resetPasswordModel.UserId);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return new ApiResult { IsSuccessful = false, Message = "Invalid email attempt." };
            }

            var result = await _userManager.ResetPasswordAsync(user, resetPasswordModel.Code, resetPasswordModel.NewPassword);
            if (result.Succeeded)
            {
                return new ApiResult { IsSuccessful = true, Message = "Password reset successful, please login." };
            }

            return new ApiResult { IsSuccessful = false, Message = result.Errors.FirstOrDefault()!.Description };
        }
        catch (Exception e)
        {
            Log.Error(e.Message);
            return new ApiResult { IsSuccessful = false, Message = "Could not reset your password, please try again." };
        }
    }

    public async Task<ApiResult> ForgotPassword(ForgotPasswordDto model)
    {
        try
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return new ApiResult { IsSuccessful = false, Message = "Invalid email attempt." };
            }

            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

            // Send confirmation email
            var queryParameters = new Dictionary<string, object>();
            queryParameters.Add("userId", user.Id);
            queryParameters.Add("code", code);

            var baseUrl = _navigationManager.BaseUri;
            var callbackUrl =
                _navigationManager.GetUriWithQueryParameters($"{baseUrl}account/resetpassword", queryParameters);

            var message = "<p>Please click on the link below to reset your password.</p>" +
                          "<p><a href=\"" + callbackUrl + "\">Reset Password</a></p>" +
                          "<br>" +
                          "<h4>" + EmailDetails.TeamNameTag + "</h4>" +
                          "<h3>" + EmailDetails.BusinessNameTag + "</h3>" +
                          "<p>Email: " + EmailDetails.SenderAddress + "</p>" +
                          "<p>Website: " + EmailDetails.Website + "</p>";

            var response = await _emailSender.SendEmailAsync(
                email: model.Email,
                subject: "Reset Password",
                body: message,
                name: EmailDetails.SenderName
            );

            Log.Information("Please check your email for the password reset link.");
            return new ApiResult() { Message = "Please check your email for the password reset link.", IsSuccessful = true };
        }
        catch (Exception e)
        {
            Log.Error(e, e.Message);
            return new ApiResult() { Message = StatusLabels.UnprocessableError };
        }
    }

    public async Task<ApiResult> ResendEmailConfirmation(ResendEmailDto model)
    {
        try
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return new ApiResult { IsSuccessful = false, Message = "Invalid email attempt." };
            }

            if (user.EmailConfirmed)
            {
                return new ApiResult { IsSuccessful = true, Message = "Email already confirmed, please login to continue." };
            }

            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

            // Send confirmation email
            var queryParameters = new Dictionary<string, object>();
            queryParameters.Add("userId", user.Id);
            queryParameters.Add("code", code);

            var baseUrl = _navigationManager.BaseUri;
            var callbackUrl = _navigationManager.GetUriWithQueryParameters($"{baseUrl}identity/account/confirmemail", queryParameters);

            var message = "<p>Please click on the link below to confirm your email address.</p>" +
                          "<p><a href=\"" + callbackUrl + "\">Confirm your email</a></p>" +
                          "<br>" +
                          "<h4>" + EmailDetails.TeamNameTag + "</h4>" +
                          "<h3>" + EmailDetails.BusinessNameTag + "</h3>" +
                          "<p>Email: " + EmailDetails.SenderAddress + "</p>" +
                          "<p>Website: " + EmailDetails.Website + "</p>";

            var response = await _emailSender.SendEmailAsync(
                email: model.Email,
                subject: "Registration Confirmation",
                body: message,
                name: EmailDetails.SenderName
            );

            Log.Information("Verification email sent. Please check your email.");
            return new ApiResult { IsSuccessful = true, Message = "Verification email sent. Please check your email." };
        }
        catch (Exception e)
        {
            Log.Error(e, e.Message);
            return new ApiResult() { Message = StatusLabels.UnprocessableError };
        }
    }

    public async Task<ApiResult> ChangeUserPasswordAsync(string userId, ChangePasswordDto model)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return new ApiResult() { Message = StatusLabels.NotFound("User") };
            }

            var changePasswordResult =
                await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);

            if (!changePasswordResult.Succeeded)
            {
                return new ApiResult() { Message = changePasswordResult.Errors.FirstOrDefault()!.Description };
            }

            Log.Information($"{user.UserName} password has been changed successfully.");
            return new ApiResult()
            {
                IsSuccessful = true,
                Message = $"{user.UserName} password has been changed successfully."
            };
        }
        catch (Exception)
        {
            return new ApiResult() { Message = StatusLabels.UnprocessableError };
        }
    }

    public async Task<ApiResults<UserRoleSectionDto>> GetUserRolesAsync(string userId)
    {
	    try
	    {
		    var userRoles = await _userRepository.GetRolesByUserAsync(userId);
		    return new ApiResults<UserRoleSectionDto>() {IsSuccessful = true, Results = userRoles};

	    }
	    catch (Exception)
	    {
		    return new ApiResults<UserRoleSectionDto>{ Message = StatusLabels.ServerErrorRecords};
	    }
    }
    
    public async Task<ApiResult> UpdateUserRolesAsync(string userid, List<UserRoleSectionDto> userRoles)
    {
	    try
	    {
		    await _userRepository.UpdateUserRolesAsync(userid, userRoles);
		    return new ApiResult() { IsSuccessful = true, Message = "Officer roles updated successfully." };
	    }
	    catch (Exception)
	    {
               
		    return new ApiResult{ Message = StatusLabels.ServerErrorRecords};
	    }
    }


    public async Task<ApiResult> AddRoleToUserAsync(UserRoleManipulationDto userRole)
       {
	       try
           {
               await _userRepository.AddRoleToUserAsync(userRole);
               Log.Information("Role {RoleName} has been added to user {UserId}", 
				   userRole.RoleName, userRole.UserId);
			   
               return new ApiResult()
			   {
				   IsSuccessful = true,
				   Message = $"Role {userRole.RoleName} has been added to Officer."
			   };
           }
           catch (Exception)
           {
	           return new ApiResult{ Message = StatusLabels.UnprocessableInsertError};
           }
       }

       public async Task<ApiResult> RemoveUserRoleAsync(UserRoleManipulationDto userRole)
       {
	       try
           {
	           await _userRepository.RemoveUserRoleAsync(userRole);
			   Log.Information("Role {RoleName} has been removed from user {UserId}", userRole.RoleName, userRole.UserId);
			   
	           return new ApiResult()
	           {
		           IsSuccessful = true,
		           Message = $"Role {userRole.RoleName} has been removed from Officer."
	           };
           }
           catch (Exception)
           {
	           return new ApiResult{ Message = StatusLabels.UnprocessableDeleteError};
           }
       }

}
