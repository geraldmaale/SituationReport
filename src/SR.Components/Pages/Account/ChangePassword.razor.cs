using Microsoft.AspNetCore.Components;
using SR.Components.DataServices;
using SR.Components.Helpers;
using SR.Shared.DTOs.User;

namespace SR.Components.Pages.Account
{
	public class ChangePasswordBase : ServiceComponentBase<ChangePasswordBase>
	{
		[Inject] public IUserDataService AccountDataService { get; set; }

		public ChangePasswordDto ChangePasswordModel { get; set; } = new();


		protected override async Task OnAfterRenderAsync(bool firstRender)
		{
			if (firstRender)
			{
				var authenticationState = await AuthStateProvider.GetAuthenticationStateAsync();
				var claimsPrincipal = authenticationState.User;
				var userId = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == "id")?.Value;
				UserAccountId = userId;
				await InvokeAsync(StateHasChanged);
			}
		}

		protected async Task HandleValidSubmit()
		{
			var result = await AccountDataService.ChangeUserPasswordAsync(UserAccountId, ChangePasswordModel);
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
}