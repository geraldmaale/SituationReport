using Microsoft.AspNetCore.Components;
using SR.Components.DataServices;
using SR.Components.Helpers;
using SR.Shared.DTOs.Crews;
using SR.Shared.DTOs.Permits;

namespace SR.Components.Pages.PermitProcessings;

public class PermitUnitDetailBase : ServiceComponentBase<PermitUnitDetailBase>
{
	[Parameter] public Guid PermitUnitId { get; set; }
	protected PermitProcessingDto? PermitUnitModel { get; set; } = new();
	[Inject] public IPermitProcessingDataService? PermitUnitDataService { get; set; }
	[Inject] public IOfficerDataService? OfficerDataService { get; set; }
    
	protected override async Task OnAfterRenderAsync(bool firstRender)
	{
		if (firstRender)
		{
			try
			{
				IsLoading = true;
				
				if (PermitUnitId != Guid.Empty)
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

	protected override void OnInitialized()
	{
		// DialogService.OnOpen += OnOpenDialog;
		DialogService.OnClose += OnEmbarkationClose;
	}

	private void OnEmbarkationClose(object obj)
	{ 
		
	}

	private async Task OnGetData()
	{
		var result = await PermitUnitDataService!
			.GetByIdAsync(PermitUnitId, CancellationTokenSource.Token);

		if (result.IsSuccessful)
		{
			MapsterMapper.Map(result.Result, PermitUnitModel);
		}
		else
		{
			ToastError(result.Message);
		}
	}
	
	protected void UpdateData()
	{
		Navigation.NavigateTo($"/permitprocessings/edit/{PermitUnitId}");
	}
	
	protected void OnCancel()
	{
		Navigation.NavigateTo("/permitprocessings");
	}
}