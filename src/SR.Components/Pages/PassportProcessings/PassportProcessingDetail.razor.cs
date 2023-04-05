using Microsoft.AspNetCore.Components;
using SR.Components.DataServices;
using SR.Components.Helpers;
using SR.Shared.DTOs.Passports;

namespace SR.Components.Pages.PassportProcessings;

public class PassportProcessingDetailBase : ServiceComponentBase<PassportProcessingDetailBase>
{
	[Parameter] public Guid PassportProcessingId { get; set; }
	protected PassportProcessingDto? PassportProcessingModel { get; set; } = new();
	[Inject] public IPassportProcessingDataService? PassportProcessingDataService { get; set; }
	[Inject] public IOfficerDataService? OfficerDataService { get; set; }
    
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
		var result = await PassportProcessingDataService!
			.GetByIdAsync(PassportProcessingId, CancellationTokenSource.Token);

		if (result.IsSuccessful)
		{
			MapsterMapper.Map(result.Result, PassportProcessingModel);
		}
		else
		{
			ToastError(result.Message);
		}
	}
	
	protected void UpdateData()
	{
		Navigation.NavigateTo($"/passportprocessings/edit/{PassportProcessingId}");
	}
	
	protected void OnCancel()
	{
		Navigation.NavigateTo("/passportprocessings");
	}
}