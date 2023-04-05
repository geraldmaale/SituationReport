using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Radzen;
using SR.Components.DataServices;
using SR.Components.Helpers;
using SR.Shared;
using SR.Shared.DTOs.Officers;

namespace SR.Components.Pages.Officers;

public class OfficerListBase: ListServiceComponentBase<OfficerListBase, OfficerDto>
{
	[Inject] public IOfficerDataService? OfficerDataService { get; set; }
	
	protected override async Task OnAfterRenderAsync(bool firstRender)
	{
		if (firstRender)
		{
			await OnLoadData();
		}
	}
	
	protected override async Task OnLoadData()
	{
		try
		{
			IsLoading = true;
			
			var result = await OfficerDataService!
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

	protected override void OnNewData()
	{
		Navigation.NavigateTo($"/officers/new");
	}

	protected void OnEditData(OfficerDto item)
	{
		Navigation.NavigateTo($"/officers/{item.OfficerId}");
	}
	
	protected override async Task OnDeleteDataAsync(OfficerDto item)
	{
		try
		{
			var dialogResult = await DialogService.Confirm($"Do you want to delete 'Officer {item.FullName}'?","Delete Officer",
				new ConfirmOptions() { OkButtonText = "Yes", CancelButtonText = "No" });
		
			if (dialogResult != null && dialogResult.Value)
			{
				var result = await OfficerDataService!.DeleteAsync(item.OfficerId, CancellationTokenSource.Token);
				if (result.IsSuccessful)
				{
					ToastSuccess(result.Message); 
					await OnLoadData();
				}
				else
				{
					ToastError(result.Message);
				}
			}
		}
		catch (Exception)
		{
			Logger.LogError(StatusLabels.UnprocessableError);
		}
	}
}