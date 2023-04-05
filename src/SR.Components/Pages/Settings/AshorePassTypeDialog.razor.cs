using Microsoft.AspNetCore.Components;
using SR.Components.DataServices;
using SR.Components.Helpers;
using SR.Shared.Entities;

namespace SR.Components.Pages.Settings;

public class AshorePassTypeDialogBase : ServiceComponentBase<AshorePassTypeDialogBase>
{
	[Parameter]
	public int AshorePassTypeId { get; set; }

	protected AshorePassType? AshorePassTypeModel { get; set; } = new();
	
	[Inject] public IAshorePassTypeDataService? AshorePassTypeDataService { get; set; }
    
	protected override async Task OnAfterRenderAsync(bool firstRender)
	{
		if (firstRender)
		{
			try
			{
				IsLoading = true;

				if (AshorePassTypeId > 0)
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
		var result = await AshorePassTypeDataService!
			.GetByIdAsync(AshorePassTypeId, CancellationTokenSource.Token);

		if (result.IsSuccessful)
		{
			MapsterMapper.Map(result.Result, AshorePassTypeModel);
		}
		else
		{
			ToastError(result.Message);
		}
	}

	private async ValueTask CreateData()
	{
		var result = await AshorePassTypeDataService!.CreateAsync(AshorePassTypeModel!, CancellationTokenSource.Token);
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
		var result = await AshorePassTypeDataService!.UpdateAsync(AshorePassTypeId, AshorePassTypeModel!, CancellationTokenSource.Token);
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
		if (AshorePassTypeId == 0)
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