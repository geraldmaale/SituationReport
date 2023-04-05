using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using SR.Components.DataServices;
using SR.Components.Helpers;
using SR.Shared;
using SR.Shared.DTOs.User;

namespace SR.Components.Pages.Account;

public class ForgottenPasswordBase : ServiceComponentBase<ForgottenPasswordBase>
{

    [Inject] public IUserDataService UserDataService { get; set; }
    protected AuthState AuthState { get; set; } = new ();
        
    protected ForgotPasswordDto ForgottenModel = new();

    protected override void OnInitialized()
    {
        AuthState.IsError = false;
        AuthState.IsSuccessful = false;
    }

    protected async Task OnValidSubmit()
    {
        try
        {
            IsSubmitted = true;

            var result = await UserDataService.ForgotPassword(ForgottenModel);
            if (result.IsSuccessful)
            {
                AuthState.IsSuccessful = true;
                AuthState.Message = result.Message;
            }
            else
            {
                AuthState.IsError = true;
                AuthState.Message = result.Message;
            }
        }
        catch (Exception)
        {
            Logger.LogError(StatusLabels.UnprocessableError);
        }
        finally
        {
            IsSubmitted = false;
        }
    }

    protected void Oninput()
    {
        IsDisabled = false;
    }
        
}