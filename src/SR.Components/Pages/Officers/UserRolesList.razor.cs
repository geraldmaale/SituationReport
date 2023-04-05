using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Serilog;
using SR.Components.DataServices;
using SR.Components.Helpers;
using SR.Shared;
using SR.Shared.DTOs.User;

namespace SR.Components.Pages.Officers
{
    public class UserRolesBase: ListServiceComponentBase<UserRolesBase, UserRoleSectionDto>
    {
        [Inject]
        public IUserDataService? UserDataService { get; set; }

        protected List<UserRoleSectionDto> UserRoleSections { get; set; } = new();

        protected override async Task OnParametersSetAsync()
        {
            await GetData();
        }

        private async Task GetData()
        {
            var result = await UserDataService.GetUserRolesAsync(
                UserId);

            if (result.IsSuccessful)
            {
                UserRoleSections = result.Results.ToList();
            }
        }

        protected async Task UpdateRoles()
        {
	        IsSubmitted = true;
	        try
	        {
		        var result = await UserDataService.UpdateUserRolesAsync(UserId, UserRoleSections);
		        if (result.IsSuccessful)
		        {
			        ToastSuccess(result.Message);
			        Navigation.NavigateTo("/officers");
		        }
		        else
		        {
			        ToastError(result.Message);
		        }
	        }
	        catch (Exception)
	        {
		        Log.Error(StatusLabels.UnprocessableError);
	        }
            finally
            {
	            IsSubmitted = false;
	            StateHasChanged();
            }
        }
    }
}