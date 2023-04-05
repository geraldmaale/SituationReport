using Microsoft.AspNetCore.Components;
using Serilog;
using SR.Components.DataServices;
using SR.Components.Helpers;
using SR.Shared;
using SR.Shared.DTOs.Officers;
using SR.Shared.Entities;

namespace SR.Components.Pages.Officers;

public class NewOfficerBase : ServiceComponentBase<NewOfficerBase>
{
	[Parameter] public Guid OfficerId { get; set; }

	protected OfficerCreationDto? OfficerModel { get; set; } = new();

	[Inject] public IOfficerDataService? OfficerDataService { get; set; }

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
			MapsterMapper.Map(result.Result, OfficerModel);
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
			var result = await OfficerDataService!.CreateAsync(OfficerModel!, CancellationTokenSource.Token);
			if (result.IsSuccessful)
			{
				ToastSuccess(result.Message);
				Navigation.NavigateTo($"/officers/{result.Result.OfficerId}");
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
		if (OfficerId == Guid.Empty)
		{
			await CreateData();
		}
	}

	protected void OnCancel()
	{
		Navigation.NavigateTo("/officers");
	}
}