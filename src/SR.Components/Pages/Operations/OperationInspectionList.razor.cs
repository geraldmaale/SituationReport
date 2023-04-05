using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Radzen;
using SR.Components.DataServices;
using SR.Components.Helpers;
using SR.Shared;
using SR.Shared.DTOs.Operations;

namespace SR.Components.Pages.Operations;

public class OperationProcessingListBase: ListServiceComponentBase<OperationProcessingListBase, OperationInspectionDto>
{
	[Parameter] public Guid OperationOfficeId { get; set; }
	[Inject] public IOperationInspectionDataService? OperationProcessingDataService { get; set; }
	[Inject] public IOperationOfficeDataService? OperationOfficeDataService { get; set; }
	protected string? OfficeName { get; set; } = "Operation Office Name";
	
	protected override async Task OnAfterRenderAsync(bool firstRender)
	{
		if (firstRender)
		{
			await GetOffice();
			await OnLoadData();
		}
	}
	
	protected override async Task OnLoadData()
	{
		try
		{
			IsLoading = true;
			
			var result = await OperationProcessingDataService!
				.GetAllAsync(OperationOfficeId, CancellationTokenSource.Token);

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

	private async ValueTask GetOffice()
	{
		try
		{
			var result = await OperationOfficeDataService!.GetByIdDtoAsync(OperationOfficeId, CancellationTokenSource.Token);
			OfficeName = result.IsSuccessful ? result.Result.OfficeName : "Operation Office";
		}
		catch (Exception)
		{
			Logger.LogError(StatusLabels.ServerErrorRecords);
		}
	}

	protected override void OnNewData()
	{
		Navigation.NavigateTo($"/operations/{OperationOfficeId}/inspection");
	}
	
	protected void OnBack()
	{
		Navigation.NavigateTo($"/operations");
	}

	protected void OnEditData(OperationInspectionDto item)
	{
		Navigation.NavigateTo($"/operations/{OperationOfficeId}/inspection/{item.Id}");
	}
	
	protected override void OnDetailData(OperationInspectionDto item)
	{
		Navigation.NavigateTo($"/operations/{OperationOfficeId}/inspection/{item.Id}/details");
	}
	
	protected override async Task OnDeleteDataAsync(OperationInspectionDto item)
	{
		try
		{
			var dialogResult = await DialogService.Confirm($"Do you want to delete this record?","Delete Revenue Collection",
				new ConfirmOptions() { OkButtonText = "Yes", CancelButtonText = "No" });
		
			if (dialogResult != null && dialogResult.Value)
			{
				var result = await OperationProcessingDataService!.DeleteAsync(OperationOfficeId, item.Id, CancellationTokenSource.Token);
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