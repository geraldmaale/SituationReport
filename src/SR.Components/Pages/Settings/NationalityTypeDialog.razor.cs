using Microsoft.AspNetCore.Components;
using SR.Components.DataServices;
using SR.Components.Helpers;
using SR.Shared.Entities;

namespace SR.Components.Pages.Settings;

public class NationalityTypeDialogBase : ServiceComponentBase<NationalityTypeDialogBase>
{
	[Parameter]
	public int NationalityTypeId { get; set; }

	protected NationalityType? NationalityTypeModel { get; set; } = new();
	
	[Inject] public INationalityTypeDataService? NationalityTypeDataService { get; set; }
    
	protected override async Task OnAfterRenderAsync(bool firstRender)
	{
		if (firstRender)
		{
			try
			{
				IsLoading = true;

				if (NationalityTypeId > 0)
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
		var result = await NationalityTypeDataService!
			.GetByIdAsync(NationalityTypeId, CancellationTokenSource.Token);

		if (result.IsSuccessful)
		{
			MapsterMapper.Map(result.Result, NationalityTypeModel);
		}
		else
		{
			ToastError(result.Message);
		}
	}

	private async ValueTask CreateData()
	{
		var result = await NationalityTypeDataService!.CreateAsync(NationalityTypeModel!, CancellationTokenSource.Token);
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
		var result = await NationalityTypeDataService!.UpdateAsync(NationalityTypeId, NationalityTypeModel!, CancellationTokenSource.Token);
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
		if (NationalityTypeId == 0)
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