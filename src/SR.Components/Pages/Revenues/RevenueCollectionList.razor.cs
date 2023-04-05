using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Radzen;
using SR.Components.DataServices;
using SR.Components.Helpers;
using SR.Shared;
using SR.Shared.DTOs.Revenues;

namespace SR.Components.Pages.Revenues;

public class RevenueCollectionListBase: ListServiceComponentBase<RevenueCollectionListBase, RevenueCollectionDto>
{
	[Inject] public IRevenueCollectionDataService? RevenueCollectionDataService { get; set; }
	
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
			
			var result = await RevenueCollectionDataService!
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
		Navigation.NavigateTo($"/revenuecollections/new");
	}

	protected void OnEditData(RevenueCollectionDto item)
	{
		Navigation.NavigateTo($"/revenuecollections/edit/{item.Id}");
	}
	
	protected override void OnDetailData(RevenueCollectionDto item)
	{
		Navigation.NavigateTo($"/revenuecollections/{item.Id}");
	}
	
	protected override async Task OnDeleteDataAsync(RevenueCollectionDto item)
	{
		try
		{
			var dialogResult = await DialogService.Confirm($"Do you want to delete this record?","Delete Revenue Collection",
				new ConfirmOptions() { OkButtonText = "Yes", CancelButtonText = "No" });
		
			if (dialogResult != null && dialogResult.Value)
			{
				var result = await RevenueCollectionDataService!.DeleteAsync(item.Id, CancellationTokenSource.Token);
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