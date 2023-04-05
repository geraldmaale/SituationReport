using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using SR.Components.DataServices;
using SR.Components.Helpers;
using SR.Shared;
using SR.Shared.DTOs.Operations;

namespace SR.Components.Pages.Operations;

public class OperationOfficeListBase: ListServiceComponentBase<OperationOfficeListBase, OperationOfficeDto>
{
	[Inject] public IOperationOfficeDataService? OperationOfficeDataService { get; set; }

	protected List<string>? Roles { get; set; }
	
	protected override async Task OnAfterRenderAsync(bool firstRender)
	{
		if (firstRender)
		{
			var authenticationState = await AuthStateProvider.GetAuthenticationStateAsync();
			var user = authenticationState.User;
			var roles = user.Claims
				.Where(c => c.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role")
				.Select(x=>x.Value)
				.ToList();
			Roles = roles;
			
			await OnLoadData();
		}
	}
	
	protected override async Task OnLoadData()
	{
		try
		{
			IsLoading = true;
            
			var result = await OperationOfficeDataService!
				.GetAllAsync(CancellationTokenSource.Token);

			if (result.IsSuccessful)
			{
				DataSource = result.Results;
			}
			else
			{
				ToastError(result.Message);
			}
		}
		catch (Exception)
		{
			Logger.LogError(StatusLabels.ServerErrorRecords);
		}
		finally
		{
			IsLoading = false;
			StateHasChanged();
		}
	}

	protected void ViewOffice(OperationOfficeDto item)
	{
		Navigation.NavigateTo($"operations/{item.OperationOfficeId}");
	}
}