using Microsoft.AspNetCore.Components;
using SR.Components.DataServices;
using SR.Components.Helpers;
using SR.Shared.DTOs.Passports;

namespace SR.Components.Pages.PassportProcessings;

public class PassportProcessingNewBase : ServiceComponentBase<PassportProcessingNewBase>
{
	[Parameter] public Guid PassportProcessingId { get; set; }
	public string? OfficerName { get; set; }
	protected PassportProcessingManipulationDto? PassportProcessingModel { get; set; } = new();
	[Inject] public IPassportProcessingDataService? PassportProcessingDataService { get; set; }
    
	protected override async Task OnAfterRenderAsync(bool firstRender)
	{
		if (firstRender)
		{
			try
			{
				IsLoading = true;
				
				if (PassportProcessingId != Guid.Empty)
				{
					await OnGetData();
				}
				else
				{
					// officerId
					var authenticationState = await AuthStateProvider.GetAuthenticationStateAsync();
					var user = authenticationState.User;
					var officerId = user.Claims.FirstOrDefault(c => c.Type == "OfficerId")?.Value;
					PassportProcessingModel!.OfficerId = Guid.Parse(officerId!);
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
	
	
	private async Task OnGetData()
	{
		var result = await PassportProcessingDataService!
			.GetByIdAsync(PassportProcessingId, CancellationTokenSource.Token);

		if (result.IsSuccessful)
		{
			MapsterMapper.Map(result.Result, PassportProcessingModel);
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
			var result =
				await PassportProcessingDataService!.CreateAsync(PassportProcessingModel!, CancellationTokenSource.Token);
			if (result.IsSuccessful)
			{
				ToastSuccess(result.Message);
				PassportProcessingModel = new();
				Navigation.NavigateTo("/passportprocessings");
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
			
			var result = await PassportProcessingDataService!.UpdateAsync(
				PassportProcessingId, PassportProcessingModel!, CancellationTokenSource.Token);
			if (result.IsSuccessful)
			{
				ToastSuccess(result.Message);
				Navigation.NavigateTo("/passportprocessings");
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
	
	protected async Task OnValidSubmit()
	{
		if (PassportProcessingId == Guid.Empty)
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
		Navigation.NavigateTo("/passportprocessings");
	}
	}