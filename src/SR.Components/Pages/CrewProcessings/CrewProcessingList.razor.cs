using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Radzen;
using SR.Components.DataServices;
using SR.Components.Helpers;
using SR.Shared;
using SR.Shared.DTOs.Crews;

namespace SR.Components.Pages.CrewProcessings;

public class CrewProcesingListBase: ListServiceComponentBase<CrewProcesingListBase, CrewProcessingDto>
{
	[Inject] public ICrewProcessingDataService? CrewProcessingDataService { get; set; }
	
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
			
			var result = await CrewProcessingDataService!
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
		Navigation.NavigateTo($"/crewprocessings/new");
	}

	protected void OnEditData(CrewProcessingDto item)
	{
		Navigation.NavigateTo($"/crewprocessings/edit/{item.Id}");
	}
	
	protected override void OnDetailData(CrewProcessingDto item)
	{
		Navigation.NavigateTo($"/crewprocessings/{item.Id}");
	}
	
	protected override async Task OnDeleteDataAsync(CrewProcessingDto item)
	{
		try
		{
			var dialogResult = await DialogService.Confirm($"Do you want to delete this record?","Delete Crew Processing",
				new ConfirmOptions() { OkButtonText = "Yes", CancelButtonText = "No" });
		
			if (dialogResult != null && dialogResult.Value)
			{
				var result = await CrewProcessingDataService!.DeleteAsync(item.Id, CancellationTokenSource.Token);
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