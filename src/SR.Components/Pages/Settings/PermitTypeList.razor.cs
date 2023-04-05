using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Radzen;
using SR.Components.DataServices;
using SR.Components.Helpers;
using SR.Shared;
using SR.Shared.DTOs.Permits;

namespace SR.Components.Pages.Settings;

public class PermitTypeListBase: ListServiceComponentBase<PermitTypeListBase, PermitTypeDto>
{
	[Inject] public IPermitTypeDataService? PermitTypeDataService { get; set; }
	
	protected override async Task OnAfterRenderAsync(bool firstRender)
	{
		if (firstRender)
		{
			await OnLoadData();
		}
	}
	
	protected override void OnInitialized()
	{
		// DialogService.OnOpen += OnOpenDialog;
		DialogService.OnClose += OnCloseDialog;
	}

	private async void OnCloseDialog(object obj)
	{
		await OnLoadData();
	}

	protected override async Task OnLoadData()
	{
		try
		{
			IsLoading = true;
            
			var result = await PermitTypeDataService!
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

	protected override async Task OnNewDataAsync()
	{
		try
		{
			await DialogService.OpenAsync<PermitTypeDialog>("Add Permit Type");
		}
		catch (Exception)
		{
			Logger.LogError(StatusLabels.UnprocessableError);
		}
	}

	protected override async Task OnEditDataAsync(PermitTypeDto item)
	{
		try
		{
			var parameters = new Dictionary<string, object>() {{"PermitTypeId", item.Id}};
			await DialogService.OpenAsync<PermitTypeDialog>("Edit Permit Type", parameters);
		}
		catch (Exception)
		{
			Logger.LogError(StatusLabels.UnprocessableError);
		}
	}
	
	protected override async Task OnDeleteDataAsync(PermitTypeDto item)
	{
		try
		{
			var dialogResult = await DialogService.Confirm($"Delete '{item.Name}'?","Delete Permit Type",
				new ConfirmOptions() { OkButtonText = "Yes", CancelButtonText = "No" });
		
			if (dialogResult != null && dialogResult.Value)
			{
				var result = await PermitTypeDataService!.DeleteAsync(item.Id, CancellationTokenSource.Token);
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