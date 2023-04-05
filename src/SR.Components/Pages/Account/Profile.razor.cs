using Microsoft.AspNetCore.Components;
using Radzen;
using SR.Components.DataServices;
using SR.Components.Helpers;
using SR.Shared.DTOs.User;

namespace SR.Components.Pages.Account;

public class ProfileBase : ServiceComponentBase<ProfileBase>
{
    public UserUpdateDto UserProfileModel { get; set; } = new();
    
    [Inject] public IUserDataService UserDataService { get; set; }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await GetData();
            await InvokeAsync(StateHasChanged);
        }
    }

    private async Task GetData()
    {
	    var authenticationState = await AuthStateProvider.GetAuthenticationStateAsync();
	    var claimsPrincipal = authenticationState.User;
	    var userId = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == "id")?.Value;
        var user = await UserDataService.FindUserByIdAsync(userId!);
        if (user.IsSuccessful)
        {
            MapsterMapper.Map(user.Result, UserProfileModel);
        }

    }

    protected async Task HandleValidSubmit()
    {
        var result = await UserDataService.UpdateUserAsync(UserAccountId, UserProfileModel);
        if (result.IsSuccessful)
        {
           ToastSuccess(result.Message);
        }
        else
        {
            ToastError(result.Message);
        }
    }
}
