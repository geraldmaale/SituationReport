using Microsoft.AspNetCore.Components;
using SR.Components.DataServices;
using SR.Components.Helpers;
using SR.Shared.Entities;

namespace SR.Components.Pages.Settings;

public class VesselTypeDialogBase : ServiceComponentBase<VesselTypeDialogBase>
{
	[Parameter]
	public int VesselTypeId { get; set; }

	protected VesselType? VesselTypeModel { get; set; } = new();
	
	[Inject] public IVesselTypeDataService? VesselTypeDataService { get; set; }
    
	protected override async Task OnAfterRenderAsync(bool firstRender)
	{
		if (firstRender)
		{
			try
			{
				IsLoading = true;

				if (VesselTypeId > 0)
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
		var result = await VesselTypeDataService!
			.GetByIdAsync(VesselTypeId, CancellationTokenSource.Token);

		if (result.IsSuccessful)
		{
			MapsterMapper.Map(result.Result, VesselTypeModel);
		}
		else
		{
			ToastError(result.Message);
		}
	}

	private async ValueTask CreateData()
	{
		var result = await VesselTypeDataService!.CreateAsync(VesselTypeModel!, CancellationTokenSource.Token);
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
		var result = await VesselTypeDataService!.UpdateAsync(VesselTypeId, VesselTypeModel!, CancellationTokenSource.Token);
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
		if (VesselTypeId == 0)
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