using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Radzen;
using SR.Components.DataServices;
using SR.Components.Helpers;
using SR.Shared;
using SR.Shared.DTOs.Permits;

namespace SR.Components.Pages.PermitProcessings;

public class PermitProcesingListBase: ListServiceComponentBase<PermitProcesingListBase, PermitProcessingDto>
{
	[Inject] public IPermitProcessingDataService? PermitUnitDataService { get; set; }
	
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
			
			var result = await PermitUnitDataService!
				.GetAllAsync(CancellationTokenSource.Token);

			if (result.IsSuccessful)
			{
				DataSource = result.Results.OrderByDescending(x=>x.EntryDate);
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
		Navigation.NavigateTo($"/permitprocessings/new");
	}

	protected void OnEditData(PermitProcessingDto item)
	{
		Navigation.NavigateTo($"/permitprocessings/edit/{item.Id}");
	}
	
	protected override void OnDetailData(PermitProcessingDto item)
	{
		Navigation.NavigateTo($"/permitprocessings/{item.Id}");
	}
	
	protected override async Task OnDeleteDataAsync(PermitProcessingDto item)
	{
		try
		{
			var dialogResult = await DialogService.Confirm($"Do you want to delete this record?","Delete Permit Processing",
				new ConfirmOptions() { OkButtonText = "Yes", CancelButtonText = "No" });
		
			if (dialogResult != null && dialogResult.Value)
			{
				var result = await PermitUnitDataService!.DeleteAsync(item.Id, CancellationTokenSource.Token);
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