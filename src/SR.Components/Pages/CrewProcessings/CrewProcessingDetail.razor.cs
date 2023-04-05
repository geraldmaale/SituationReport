using Microsoft.AspNetCore.Components;
using SR.Components.DataServices;
using SR.Components.Helpers;
using SR.Shared.DTOs.Crews;

namespace SR.Components.Pages.CrewProcessings;

public class CrewProcessingDetailBase : ServiceComponentBase<CrewProcessingDetailBase>
{
	[Parameter] public Guid CrewProcessingId { get; set; }
	public string? OfficerName { get; set; }
	protected CrewProcessingManipulationDto? CrewProcessingModel { get; set; } = new();
	[Inject] public ICrewProcessingDataService? CrewProcessingDataService { get; set; }
	[Inject] public IOfficerDataService? OfficerDataService { get; set; }
    
	protected override async Task OnAfterRenderAsync(bool firstRender)
	{
		if (firstRender)
		{
			try
			{
				IsLoading = true;
				// officerId
				var authenticationState = await AuthStateProvider.GetAuthenticationStateAsync();
				var user = authenticationState.User;
				var officerId = user.Claims.FirstOrDefault(c => c.Type == "OfficerId")?.Value;
				CrewProcessingModel!.OfficerId = Guid.Parse(officerId!);
				OfficerName = user.Claims.FirstOrDefault(c => c.Type == "name")?.Value;

				if (CrewProcessingId != Guid.Empty)
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
		var result = await CrewProcessingDataService!
			.GetByIdAsync(CrewProcessingId, CancellationTokenSource.Token);

		if (result.IsSuccessful)
		{
			MapsterMapper.Map(result.Result, CrewProcessingModel);
			// Get officer
			var officer = await OfficerDataService!.GetByIdAsync(
				result.Result.OfficerId, CancellationTokenSource.Token);
			OfficerName = officer.Result.FullName;
		}
		else
		{
			ToastError(result.Message);
		}
	}
	
	protected void UpdateData()
	{
		Navigation.NavigateTo($"/crewprocessings/edit/{CrewProcessingId}");
	}
	
	protected void OnCancel()
	{
		Navigation.NavigateTo("/crewprocessings");
	}
}