using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Radzen;
using SR.Components.DataServices;
using SR.Components.Helpers;
using SR.Shared;
using SR.Shared.Entities;

namespace SR.Components.Pages.Settings;

public class OperationTypeListBase: ListServiceComponentBase<OperationTypeListBase, OperationType>
{
	[Inject] public IOperationTypeDataService? OperationTypeDataService { get; set; }
	
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
            
			var result = await OperationTypeDataService!
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
			await DialogService.OpenAsync<OperationTypeDialog>("Add Operation Type");
		}
		catch (Exception)
		{
			Logger.LogError(StatusLabels.UnprocessableError);
		}
	}

	protected override async Task OnEditDataAsync(OperationType item)
	{
		try
		{
			var parameters = new Dictionary<string, object>() {{"OperationTypeId", item.Id}};
			await DialogService.OpenAsync<OperationTypeDialog>("Edit Operation Type", parameters);
		}
		catch (Exception)
		{
			Logger.LogError(StatusLabels.UnprocessableError);
		}
	}
	
	protected override async Task OnDeleteDataAsync(OperationType item)
	{
		try
		{
			var dialogResult = await DialogService.Confirm($"Delete '{item.Name}'?","Delete Operation Type",
				new ConfirmOptions() { OkButtonText = "Yes", CancelButtonText = "No" });
		
			if (dialogResult != null && dialogResult.Value)
			{
				var result = await OperationTypeDataService!.DeleteAsync(item.Id, CancellationTokenSource.Token);
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