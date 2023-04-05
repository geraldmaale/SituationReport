using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using SR.Components.DataServices;
using SR.Components.Helpers;
using SR.Shared;
using SR.Shared.DTOs.Operations;

namespace SR.Components.Pages.Operations;

public class OperationInspectionDetailsBase : ServiceComponentBase<OperationInspectionDetailsBase>
{
	[Parameter] public Guid OperationInspectionId { get; set; }
	[Parameter] public Guid OperationOfficeId { get; set; }
	protected string? OfficerName { get; set; }
	protected string? OfficeName { get; set; }
	protected OperationInspectionManipulationDto? OperationInspectionModel { get; set; } = new();
	[Inject] public IOperationInspectionDataService? OperationInspectionDataService { get; set; }
	[Inject] public IOperationTypeDataService? OperationTypeDataService { get; set; }
	[Inject] public IOperationOfficeDataService? OperationOfficeDataService { get; set; }
    
	protected override async Task OnAfterRenderAsync(bool firstRender)
	{
		if (firstRender)
		{
			try
			{
				IsLoading = true;
				await GetOffice();
				
				if (OperationInspectionId != Guid.Empty)
				{
					await OnGetData();
				}
			}
			finally
			{
				IsLoading = false;
				StateHasChanged();
			}
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
	
	private async Task OnGetData()
	{
		var result = await OperationInspectionDataService!
			.GetByIdAsync(OperationOfficeId, OperationInspectionId, CancellationTokenSource.Token);

		if (result.IsSuccessful)
		{
			MapsterMapper.Map(result.Result, OperationInspectionModel);
			OfficerName = result.Result.Officer.FullName;
		}
		else
		{
			ToastError(result.Message);
		}
	}
	
	protected void UpdateData()
	{
		Navigation.NavigateTo($"/Operations/{OperationOfficeId}/inspection/{OperationInspectionId}");
	}
	
	protected void OnCancel()
	{
		Navigation.NavigateTo($"/Operations/{OperationOfficeId}");
	}
}