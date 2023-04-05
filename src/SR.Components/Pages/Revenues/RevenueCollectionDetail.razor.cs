using Microsoft.AspNetCore.Components;
using SR.Components.DataServices;
using SR.Components.Helpers;
using SR.Shared.DTOs.Revenues;

namespace SR.Components.Pages.Revenues;

public class RevenueCollectionDetailBase : ServiceComponentBase<RevenueCollectionDetailBase>
{
	[Parameter] public Guid RevenueCollectionId { get; set; }
	protected RevenueCollectionDto? RevenueCollectionModel { get; set; } = new();
	[Inject] public IRevenueCollectionDataService? RevenueCollectionDataService { get; set; }
	[Inject] public IOfficerDataService? OfficerDataService { get; set; }
    
	protected override async Task OnAfterRenderAsync(bool firstRender)
	{
		if (firstRender)
		{
			try
			{
				IsLoading = true;
				
				if (RevenueCollectionId != Guid.Empty)
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
		var result = await RevenueCollectionDataService!
			.GetByIdAsync(RevenueCollectionId, CancellationTokenSource.Token);

		if (result.IsSuccessful)
		{
			MapsterMapper.Map(result.Result, RevenueCollectionModel);
		}
		else
		{
			ToastError(result.Message);
		}
	}
	
	protected void UpdateData()
	{
		Navigation.NavigateTo($"/revenuecollections/edit/{RevenueCollectionId}");
	}
	
	protected void OnCancel()
	{
		Navigation.NavigateTo("/revenuecollections");
	}
}