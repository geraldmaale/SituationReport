using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Radzen;
using SR.Components.DataServices;
using SR.Components.Helpers;
using SR.Shared;
using SR.Shared.DTOs.Passports;

namespace SR.Components.Pages.PassportProcessings;

public class PassportProcesingListBase: ListServiceComponentBase<PassportProcesingListBase, PassportProcessingDto>
{
	[Inject] public IPassportProcessingDataService? PassportProcessingDataService { get; set; }
	
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
			
			var result = await PassportProcessingDataService!
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
		Navigation.NavigateTo($"/passportprocessings/new");
	}

	protected void OnEditData(PassportProcessingDto item)
	{
		Navigation.NavigateTo($"/passportprocessings/edit/{item.Id}");
	}
	
	protected override void OnDetailData(PassportProcessingDto item)
	{
		Navigation.NavigateTo($"/passportprocessings/{item.Id}");
	}
	
	protected override async Task OnDeleteDataAsync(PassportProcessingDto item)
	{
		try
		{
			var dialogResult = await DialogService.Confirm($"Do you want to delete this record?","Delete Passport Processing",
				new ConfirmOptions() { OkButtonText = "Yes", CancelButtonText = "No" });
		
			if (dialogResult != null && dialogResult.Value)
			{
				var result = await PassportProcessingDataService!.DeleteAsync(item.Id, CancellationTokenSource.Token);
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