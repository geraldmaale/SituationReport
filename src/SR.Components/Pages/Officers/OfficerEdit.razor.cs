using Microsoft.AspNetCore.Components;
using Radzen;
using Serilog;
using SR.Components.DataServices;
using SR.Components.Helpers;
using SR.Shared;
using SR.Shared.DTOs.Officers;
using SR.Shared.Entities;

namespace SR.Components.Pages.Officers;

public class OfficerEditBase : ServiceComponentBase<OfficerEditBase>
{
	[Parameter] public Guid OfficerId { get; set; }

	protected OfficerUpdateDto? OfficerModel { get; set; } = new();
	protected OfficerFullDto? Officer { get; set; } = new();
	
	[Inject] public IOfficerDataService? OfficerDataService { get; set; }
	[Inject] public IUserDataService? UserDataService { get; set; }
    
	protected override async Task OnAfterRenderAsync(bool firstRender)
	{
		if (firstRender)
		{
			try
			{
				IsLoading = true;

				if (OfficerId != Guid.Empty)
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

	private async Task OnGetData()
	{
		var result = await OfficerDataService!
			.GetByIdAsync(OfficerId, CancellationTokenSource.Token);

		if (result.IsSuccessful)
		{
			Officer = result.Result;
			UserId = Officer.UserId;
			MapsterMapper.Map(result.Result, OfficerModel);
		}
		else
		{
			ToastError(result.Message);
		}
	}
	
	private async ValueTask UpdateData()
	{
		try
		{
			IsSubmitted = true;

			if (!string.IsNullOrWhiteSpace(OfficerModel!.Username) 
			    && string.IsNullOrWhiteSpace(Officer!.Username) 
			    && string.IsNullOrWhiteSpace(OfficerModel.Password))
			{
				ToastError("Password is required.");
				return;
			}
			
			var updateModel = MapsterMapper.Map<OfficerUpdateDto>(OfficerModel!);

			var result = await OfficerDataService!.UpdateAsync(OfficerId, updateModel!, CancellationTokenSource.Token);
			if (result.IsSuccessful)
			{
				ToastSuccess(result.Message);
				Navigation.NavigateTo("/officers");
			}
			else
			{
				ToastError(result.Message);
			}
		}
		catch (Exception)
		{
			Log.Error(StatusLabels.UnprocessableError);
		}
		finally
		{
			IsSubmitted = false;
			StateHasChanged();
		}
	}
	
	protected async Task OnValidSubmit()
	{
		await UpdateData();
	}
	
	protected void OnCancel()
	{
		Navigation.NavigateTo("/officers");
	}
	
	protected async Task OnRemoveAccount()
	{
		var dialogResult = await DialogService.Confirm($"Do you want to remove the login account?","Remove login account",
			new ConfirmOptions() { OkButtonText = "Yes", CancelButtonText = "No" });
		
		if (dialogResult != null && dialogResult.Value)
		{
			var result = await UserDataService!.DeleteUserByUsernameAsync(OfficerModel!.Username);
			if (result.IsSuccessful)
			{
				ToastSuccess(result.Message); 
				OfficerModel.Username = string.Empty;
				await OfficerDataService!.UpdateAsync(OfficerId, OfficerModel!, CancellationTokenSource.Token);
				Navigation.NavigateTo("/officers");
			}
			else
			{
				ToastError(result.Message);
			}
		}
	}
}