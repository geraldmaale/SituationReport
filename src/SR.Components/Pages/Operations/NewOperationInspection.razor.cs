using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Radzen;
using Radzen.Blazor;
using SR.Components.DataServices;
using SR.Components.Helpers;
using SR.Shared;
using SR.Shared.DTOs.Operations;
using SR.Shared.Entities;

namespace SR.Components.Pages.Operations;

public class OperationInspectionsNewBase : ServiceComponentBase<OperationInspectionsNewBase>
{
	[Parameter] public Guid OperationInspectionId { get; set; }
	[Parameter] public Guid OperationOfficeId { get; set; }
	protected string? OfficeName { get; set; } = "Operation Office Name";
	protected string? OfficerName { get; set; }
	protected RadzenDataGrid<OperationInspectionDetailDto>? DataGrid { get; set; }
	protected OperationInspectionManipulationDto? OperationInspectionModel { get; set; } = new();
	protected IEnumerable<OperationType>? OperationTypes { get; set; } = new List<OperationType>();
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
				await OperationTypesLookup();
				
				if (OperationInspectionId != Guid.Empty)
				{
					await OnGetData();
				}
				else
				{
					// officerId
					var authenticationState = await AuthStateProvider.GetAuthenticationStateAsync();
					var user = authenticationState.User;
					var officerId = user.Claims.FirstOrDefault(c => c.Type == "OfficerId")?.Value;
					OperationInspectionModel!.OfficerId = Guid.Parse(officerId!);
					OfficerName = user.Claims.FirstOrDefault(c => c.Type == "name")?.Value;
				}
			}
			finally
			{
				IsLoading = false;
				StateHasChanged();
			}
		}
	}
	
	private async Task OperationTypesLookup()
	{
		try
		{
			IsLoading = true;
            
			var result = await OperationTypeDataService!
				.GetAllAsync(CancellationTokenSource.Token);

			if (result.IsSuccessful)
			{
				OperationTypes = result.Results;
			}
			else
			{
				ToastError(result.Message);
			}
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

	private async ValueTask CreateData()
	{
		try
		{
			IsSubmitted = true;
			OperationInspectionModel!.OperationOfficeId = OperationOfficeId;
			var result =
				await OperationInspectionDataService!.CreateAsync(OperationOfficeId, OperationInspectionModel!, CancellationTokenSource.Token);
			if (result.IsSuccessful)
			{
				ToastSuccess(result.Message);
				OperationInspectionModel = new();
				Navigation.NavigateTo($"/Operations/{OperationOfficeId}");
			}
			else
			{
				ToastError(result.Message);
			}
		}
		finally
		{
			IsSubmitted = false;
			StateHasChanged();
		}
	}
	
	private async ValueTask UpdateData()
	{
		try
		{
			IsSubmitted = true;
			
			var result = await OperationInspectionDataService!.UpdateAsync(OperationOfficeId, OperationInspectionId, OperationInspectionModel!, CancellationTokenSource.Token);
			if (result.IsSuccessful)
			{
				ToastSuccess(result.Message);
				Navigation.NavigateTo($"/Operations/{OperationOfficeId}");
			}
			else
			{
				ToastError(result.Message);
			}
		}
		finally
		{
			IsSubmitted = false;
			StateHasChanged();
		}
	}

	protected async Task OnDeleteOperationTypeAsync(OperationInspectionDetailDto item)
	{
		try
		{
			var dialogResult = await DialogService.Confirm($"Do you want to Revenue type data?", "Revenue Collection",
				new ConfirmOptions() {OkButtonText = "Yes", CancelButtonText = "No"});

			if (dialogResult != null && dialogResult.Value)
			{
				OperationInspectionModel!.OperationInspectionDetails.Remove(item);
				await DataGrid!.Reload();
			}
		}
		catch (Exception)
		{
			Logger.LogError(StatusLabels.UnprocessableError);
		}
	}

	protected async Task OnValidSubmit()
	{
		// Validate and aggregate duplicated data
		var details = OperationInspectionModel!.OperationInspectionDetails.Aggregate(new List<OperationInspectionDetailDto>(),
			(list, item) =>
			{
				if (list.All(x => x.OperationTypeId != item.OperationTypeId))
				{
					list.Add(item);
				}
				else
				{
					list.First(x => x.OperationTypeId == item.OperationTypeId).Number += item.Number;
				}

				return list;
			});
		
		OperationInspectionModel.OperationInspectionDetails = details;
		
		if (OperationInspectionId == Guid.Empty)
		{
			await CreateData();
		}
		else
		{
			await UpdateData();
		}
	}
	
	protected void OnCancel()
	{
		Navigation.NavigateTo($"/Operations/{OperationOfficeId}");
	}
	
	protected async Task AddRevenueRecord(OperationInspectionDetailDto item)
	{
		await DataGrid!.InsertRow(item);
		OperationInspectionModel!.OperationInspectionDetails.Add(item);
	}
}