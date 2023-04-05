using Microsoft.AspNetCore.Components;
using SR.Components.DataServices;
using SR.Components.Helpers;
using SR.Shared;
using SR.Shared.DTOs.Permits;

namespace SR.Components.Pages.Settings;

public class PermitTypeDialogBase : ServiceComponentBase<PermitTypeDialogBase>
{
	[Parameter]
	public int PermitTypeId { get; set; }

	protected PermitTypeManipulationDto? PermitTypeModel { get; set; } = new();
	
	[Inject] public IPermitTypeDataService? PermitTypeDataService { get; set; }
    
	protected override async Task OnAfterRenderAsync(bool firstRender)
	{
		if (firstRender)
		{
			try
			{
				IsLoading = true;

				if (PermitTypeId > 0)
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
		var result = await PermitTypeDataService!
			.GetByIdAsync(PermitTypeId, CancellationTokenSource.Token);

		if (result.IsSuccessful)
		{
			MapsterMapper.Map(result.Result, PermitTypeModel);
		}
		else
		{
			ToastError(result.Message);
		}
	}

	private async ValueTask CreateData()
	{
		var result = await PermitTypeDataService!.CreateAsync(PermitTypeModel!, CancellationTokenSource.Token);
		if (result.IsSuccessful)
		{
			ToastSuccess(result.Message);
			DialogService.Close(this);
		}
		else
		{
			ToastError(result.Message);
		}
	}
	
	private async ValueTask UpdateData()
	{
		var result = await PermitTypeDataService!.UpdateAsync(PermitTypeId, PermitTypeModel!, CancellationTokenSource.Token);
		if (result.IsSuccessful)
		{
			ToastSuccess(result.Message);
			DialogService.Close(this);
		}
		else
		{
			ToastError(result.Message);
		}
	}
	
	protected async Task OnValidSubmit()
	{
		if (PermitTypeId == 0)
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
		DialogService.Close(this);
	}
}