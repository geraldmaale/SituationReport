using System.Text;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;
using SR.Components.DataServices;
using SR.Components.Helpers;
using SR.Shared;
using SR.Shared.DTOs.User;

namespace SR.Components.Pages.Account
{
    public class ResetPasswordBase : ServiceComponentBase<ResetPasswordBase>
    {
        [Inject] public IUserDataService UserDataService { get; set; }
        protected AuthState AuthState { get; set; } = new ();
        protected ResetPasswordDto ResetPasswordModel { get; set; } = new ();
        
        [Parameter] 
        [SupplyParameterFromQuery]
        public string code { get; set; }
        
        [Parameter] 
        [SupplyParameterFromQuery]
        public string userId { get; set; }

        protected override void OnParametersSet()
        {
            AuthState.IsError = false;
            AuthState.IsSuccessful = false;

            if (!string.IsNullOrWhiteSpace(code))
            {
                var resetCode = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
                ResetPasswordModel.Code = resetCode;
                ResetPasswordModel.UserId = userId;
            }
            else
            {
                AuthState.IsError = true;
                AuthState.Message = "Invalid request";
            }
        }

        protected async Task OnValidSubmit()
        {
            AuthState.IsError = false;
            var result = await UserDataService.ResetPassword(ResetPasswordModel);
            if (result.IsSuccessful)
            {
                AuthState.IsSuccessful = true;
                AuthState.Message = result.Message;
            }
            else
            {
                AuthState.IsError = true;
                AuthState.Message = "Sorry, token has expired or is invalid. Please request a new one.";
            }

            StateHasChanged();
        }
    }
}