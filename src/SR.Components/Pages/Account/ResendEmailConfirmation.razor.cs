using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using SR.Components.DataServices;
using SR.Components.Helpers;
using SR.Shared;
using SR.Shared.DTOs.User;

namespace SR.Components.Pages.Account;

public class ResendEmailConfirmationBase : ServiceComponentBase<ResendEmailConfirmationBase>
{
    protected AuthState AuthState { get; set; } = new ();
    [Inject] public IUserDataService UserDataService { get; set; }
        
    protected ResendEmailDto ResendEmailModel = new();

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
            
            AuthState.IsError = false;
            AuthState.IsSuccessful = false;

            var result = await UserDataService.ResendEmailConfirmation(ResendEmailModel);
            if (result.IsSuccessful)
            {
                AuthState.IsSuccessful = true;
                AuthState.Message = result.Message;
            }
            else
            {
                AuthState.IsError = true;
                AuthState.Message = result.Message;
                // ToastError(result.Message);
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