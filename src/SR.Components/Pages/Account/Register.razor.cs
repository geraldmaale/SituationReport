using Microsoft.AspNetCore.Components;
using SR.Components.DataServices;
using SR.Components.Helpers;
using SR.Shared;
using SR.Shared.DTOs.User;

namespace SR.Components.Pages.Account
{
    public class RegisterBase: ServiceComponentBase<RegisterBase>
    {
        [Inject]
        public IUserDataService  UserDataService { get; set; }

        protected RegisterDto RegisterModel { get; set; } = new();
        
        protected AuthState AuthState { get; set; } = new ();

        protected override void OnInitialized()
        {
            AuthState.IsError = false;
            AuthState.IsSuccessful = false;
        }

        protected async Task OnSubmitted()
        {
            try
            {
                IsSubmitted = true;
                
                var result = await UserDataService.Register(RegisterModel);
                if (result.IsSuccessful)
                {
                    AuthState.IsSuccessful = true;
                    AuthState.IsError = false;
                    AuthState.Message = result.Message;
                }
                else
                {
                    AuthState.IsError = true;
                    AuthState.Message = result.Message;
                }
            }
            finally
            {
                IsSubmitted = false;
                StateHasChanged();
            }
        }
    }
}