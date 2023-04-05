using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Radzen;
using SR.Components.DataServices;
using SR.Components.Helpers;
using SR.Shared;
using SR.Shared.DTOs.Permits;
using SR.Shared.Entities;

namespace SR.Components.Pages.PermitProcessings;

public class PermitProcessingNewBase : ServiceComponentBase<PermitProcessingNewBase>
{
	[Parameter] public Guid PermitProcessingId { get; set; }
	public string? OfficerName { get; set; }
	protected PermitProcessingManipulationDto? PermitUnitModel { get; set; } = new();
	protected IEnumerable<NationalityType>? NationalityTypes { get; set; } = new List<NationalityType>();
	protected IEnumerable<PermitTypeDto>? PermitTypes { get; set; } = new List<PermitTypeDto>();
	
	[Inject] public INationalityTypeDataService? NationalityTypeDataService { get; set; }
	[Inject] public IPermitTypeDataService? PermitTypeDataService { get; set; }
	
	[Inject] public IPermitProcessingDataService? PermitProcessingDataService { get; set; }
	[Inject] public IOfficerDataService? OfficerDataService { get; set; }
    
	protected override async Task OnAfterRenderAsync(bool firstRender)
	{
		if (firstRender)
		{
			try
			{
				IsLoading = true;
				
				await NationalitiesLookup();
				await PermitTypesLookup();
				
				if (PermitProcessingId != Guid.Empty)
				{
					await OnGetData();
				}
				else
				{
					// officerId
					var authenticationState = await AuthStateProvider.GetAuthenticationStateAsync();
					var user = authenticationState.User;
					var officerId = user.Claims.FirstOrDefault(c => c.Type == "OfficerId")?.Value;
					PermitUnitModel!.OfficerId = Guid.Parse(officerId!);
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
	
	private async Task NationalitiesLookup()
	{
		try
		{
			IsLoading = true;
            
			var result = await NationalityTypeDataService!
				.GetAllAsync(CancellationTokenSource.Token);

			if (result.IsSuccessful)
			{
				NationalityTypes = result.Results;
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
	
	private async Task PermitTypesLookup()
	{
		try
		{
			IsLoading = true;
            
			var result = await PermitTypeDataService!
				.GetAllAsync(CancellationTokenSource.Token);

			if (result.IsSuccessful)
			{
				PermitTypes = result.Results;
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
	
	private async Task OnGetData()
	{
		var result = await PermitProcessingDataService!
			.GetByIdAsync(PermitProcessingId, CancellationTokenSource.Token);

		if (result.IsSuccessful)
		{
			MapsterMapper.Map(result.Result, PermitUnitModel);
			// Get officer
			var officer = await OfficerDataService!.GetByIdAsync(
				result.Result.OfficerId, CancellationTokenSource.Token);
			OfficerName = officer.Result.FullName;
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
			var result =
				await PermitProcessingDataService!.CreateAsync(PermitUnitModel!, CancellationTokenSource.Token);
			if (result.IsSuccessful)
			{
				ToastSuccess(result.Message);
				PermitUnitModel = new();
				Navigation.NavigateTo("/permitprocessings");
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
			
			var result = await PermitProcessingDataService!.UpdateAsync(
				PermitProcessingId, PermitUnitModel!, CancellationTokenSource.Token);
			if (result.IsSuccessful)
			{
				ToastSuccess(result.Message);
				Navigation.NavigateTo("/permitprocessings");
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

	protected async Task OnDeletePermitAsync(PermitProcessingDetailManipulationDto item)
	{
		try
		{
			var dialogResult = await DialogService.Confirm($"Do you want to permit data?", "Delete Permit Data",
				new ConfirmOptions() {OkButtonText = "Yes", CancelButtonText = "No"});

			if (dialogResult != null && dialogResult.Value)
			{
				PermitUnitModel!.PermitProcessingDetails.Remove(item);
				await PermitProcessingDataService!.DeleteDetailAsync(item.PermitProcessingDetailId, PermitProcessingId, CancellationTokenSource.Token);
			}
		}
		catch (Exception)
		{
			Logger.LogError(StatusLabels.UnprocessableError);
		}
	}

	protected async Task OnValidSubmit()
	{
		if (PermitProcessingId == Guid.Empty)
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
		Navigation.NavigateTo("/permitprocessings");
	}
	
	protected void AddPermitRecord()
	{
		PermitUnitModel!.PermitProcessingDetails.Add(new ());
	}
}