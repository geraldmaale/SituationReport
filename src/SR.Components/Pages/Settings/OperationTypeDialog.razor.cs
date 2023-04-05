using Microsoft.AspNetCore.Components;
using SR.Components.DataServices;
using SR.Components.Helpers;
using SR.Shared.Entities;

namespace SR.Components.Pages.Settings;

public class OperationTypeDialogBase : ServiceComponentBase<OperationTypeDialogBase>
{
	[Parameter]
	public int OperationTypeId { get; set; }

	protected OperationType? OperationTypeModel { get; set; } = new();
	
	[Inject] public IOperationTypeDataService? OperationTypeDataService { get; set; }
    
	protected override async Task OnAfterRenderAsync(bool firstRender)
	{
		if (firstRender)
		{
			try
			{
				IsLoading = true;

				if (OperationTypeId > 0)
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
		var result = await OperationTypeDataService!
			.GetByIdAsync(OperationTypeId, CancellationTokenSource.Token);

		if (result.IsSuccessful)
		{
			MapsterMapper.Map(result.Result, OperationTypeModel);
		}
		else
		{
			ToastError(result.Message);
		}
	}

	private async ValueTask CreateData()
	{
		var result = await OperationTypeDataService!.CreateAsync(OperationTypeModel!, CancellationTokenSource.Token);
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
		var result = await OperationTypeDataService!.UpdateAsync(OperationTypeId, OperationTypeModel!, CancellationTokenSource.Token);
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
		if (OperationTypeId == 0)
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