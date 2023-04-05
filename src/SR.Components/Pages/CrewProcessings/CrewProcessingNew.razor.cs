using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using NuGet.Packaging;
using Radzen;
using Radzen.Blazor;
using SR.Components.DataServices;
using SR.Components.Helpers;
using SR.Shared;
using SR.Shared.DTOs.Crews;
using SR.Shared.Entities;

namespace SR.Components.Pages.CrewProcessings;

public class CrewProcessingNewBase : ServiceComponentBase<CrewProcessingNewBase>
{
	[Parameter] public Guid CrewProcessingId { get; set; }
	public string? OfficerName { get; set; }
	protected CrewProcessingManipulationDto? CrewProcessingModel { get; set; } = new();
	protected IEnumerable<NationalityType>? NationalityTypes { get; set; } = new List<NationalityType>();
	protected IEnumerable<AshorePassType>? AshorePassTypes { get; set; } = new List<AshorePassType>();
	protected IEnumerable<VesselType>? VesselTypes { get; set; } = new List<VesselType>();
	
	[Inject] public INationalityTypeDataService? NationalityTypeDataService { get; set; }
	[Inject] public IVesselTypeDataService? VesselTypeDataService { get; set; }
	[Inject] public IAshorePassTypeDataService? AshorePassTypeDataService { get; set; }
	[Inject] public ICrewProcessingDataService? CrewProcessingDataService { get; set; }
	[Inject] public IOfficerDataService? OfficerDataService { get; set; }
    
	protected override async Task OnAfterRenderAsync(bool firstRender)
	{
		if (firstRender)
		{
			try
			{
				IsLoading = true;
				
				await NationalitiesLookup();
				await VesselsLookup();
				await AshorePassLookup();
				
				if (CrewProcessingId != Guid.Empty)
				{
					await OnGetData();
				}
				else
				{
					// officerId
					var authenticationState = await AuthStateProvider.GetAuthenticationStateAsync();
					var user = authenticationState.User;
					var officerId = user.Claims.FirstOrDefault(c => c.Type == "OfficerId")?.Value;
					CrewProcessingModel!.OfficerId = Guid.Parse(officerId!);
					OfficerName = user.Claims.FirstOrDefault(c => c.Type == "name")?.Value;
				}
			}
			finally
			{
				IsLoading = false;
				StateHasChanged();
			}
		}
	}
	
	private async Task NationalitiesLookup()
	{
		try
		{
			IsLoading = true;
            
			var result = await NationalityTypeDataService!
				.GetAllAsync(CancellationTokenSource.Token);

			if (result.IsSuccessful)
			{
				NationalityTypes = result.Results;
			}
			else
			{
				ToastError(result.Message);
			}
		}
		finally
		{
			IsLoading = false;
			StateHasChanged();
		}
	}
	
	private async Task VesselsLookup()
	{
		try
		{
			IsLoading = true;
            
			var result = await VesselTypeDataService!
				.GetAllAsync(CancellationTokenSource.Token);

			if (result.IsSuccessful)
			{
				VesselTypes = result.Results;
			}
			else
			{
				ToastError(result.Message);
			}
		}
		finally
		{
			IsLoading = false;
			StateHasChanged();
		}
	}
	
	private async Task AshorePassLookup()
	{
		try
		{
			IsLoading = true;
            
			var result = await AshorePassTypeDataService!
				.GetAllAsync(CancellationTokenSource.Token);

			if (result.IsSuccessful)
			{
				AshorePassTypes = result.Results;
			}
			else
			{
				ToastError(result.Message);
			}
		}
		finally
		{
			IsLoading = false;
			StateHasChanged();
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

	private async ValueTask CreateData()
	{
		try
		{
			IsSubmitted = true;
			var result =
				await CrewProcessingDataService!.CreateAsync(CrewProcessingModel!, CancellationTokenSource.Token);
			if (result.IsSuccessful)
			{
				ToastSuccess(result.Message);
				CrewProcessingModel = new();
				Navigation.NavigateTo("/crewprocessings");
			}
			else
			{
				ToastError(result.Message);
			}
		}
		finally
		{
			IsSubmitted = false;
			StateHasChanged();
		}
	}
	
	private async ValueTask UpdateData()
	{
		try
		{
			IsSubmitted = true;
			
			var result = await CrewProcessingDataService!.UpdateAsync(
				CrewProcessingId, CrewProcessingModel!, CancellationTokenSource.Token);
			if (result.IsSuccessful)
			{
				ToastSuccess(result.Message);
				Navigation.NavigateTo("/crewprocessings");
			}
			else
			{
				ToastError(result.Message);
			}
		}
		finally
		{
			IsSubmitted = false;
			StateHasChanged();
		}
	}

	protected async Task OnDeleteEmbarkationAsync(EmbarkationDto item)
	{
		try
		{
			var dialogResult = await DialogService.Confirm($"Do you want to delete embarkation data?", "Delete Embarkation",
				new ConfirmOptions() {OkButtonText = "Yes", CancelButtonText = "No"});

			if (dialogResult != null && dialogResult.Value)
			{
				CrewProcessingModel!.Embarkations.Remove(item);
				await CrewProcessingDataService!.DeleteEmbarkationAsync(item.EmbarkationId, CrewProcessingId, CancellationTokenSource.Token);
			}
		}
		catch (Exception)
		{
			Logger.LogError(StatusLabels.UnprocessableError);
		}
	}

	protected async Task OnDeleteDisEmbarkationAsync(DisEmbarkationDto item)
	{
		try
		{
			var dialogResult = await DialogService.Confirm($"Do you want to delete disembarkation data?", "Delete DisEmbarkation",
				new ConfirmOptions() {OkButtonText = "Yes", CancelButtonText = "No"});

			if (dialogResult != null && dialogResult.Value)
			{
				CrewProcessingModel!.DisEmbarkations.Remove(item);
				await CrewProcessingDataService!.DeleteDisEmbarkationAsync(item.DisEmbarkationId, CrewProcessingId, CancellationTokenSource.Token);
			}
		}
		catch (Exception)
		{
			Logger.LogError(StatusLabels.UnprocessableError);
		}
	}
	
	protected async Task OnDeleteVesselDockedAsync(VesselsDockedDto item)
	{
		try
		{
			var dialogResult = await DialogService.Confirm($"Do you want to delete vessel docked data?", "Delete Vessel Docked",
				new ConfirmOptions() {OkButtonText = "Yes", CancelButtonText = "No"});

			if (dialogResult != null && dialogResult.Value)
			{
				CrewProcessingModel!.VesselsDocked.Remove(item);
				await CrewProcessingDataService!.DeleteVesselDockedAsync(item.VesselDockedId, CrewProcessingId, CancellationTokenSource.Token);
			}
		}
		catch (Exception)
		{
			Logger.LogError(StatusLabels.UnprocessableError);
		}
	}
	
	protected async Task OnDeleteAshorePassAsync(AshorePassDto item)
	{
		try
		{
			var dialogResult = await DialogService.Confirm($"Do you want to ashore pass data?", "Delete Ashore Pass",
				new ConfirmOptions() {OkButtonText = "Yes", CancelButtonText = "No"});

			if (dialogResult != null && dialogResult.Value)
			{
				CrewProcessingModel!.AshorePasses.Remove(item);
				await CrewProcessingDataService!.DeleteVesselDockedAsync(item.AshorePassId, CrewProcessingId, CancellationTokenSource.Token);
			}
		}
		catch (Exception)
		{
			Logger.LogError(StatusLabels.UnprocessableError);
		}
	}

	protected async Task OnValidSubmit()
	{
		// int total = 0;
		// Validate and aggregate duplicated data
		var embarkations = CrewProcessingModel!.Embarkations.Aggregate(new List<EmbarkationDto>(),
			(list, item) =>
			{
				if (list.All(x => x.NationalityId != item.NationalityId))
				{
					list.Add(item);
				}
				else
				{
					list.First(x => x.NationalityId == item.NationalityId).Count += item.Count;
				}

				// total = list.Sum(x => x.Count);
				return list;
			});
		
		var disEmbarkations = CrewProcessingModel!.DisEmbarkations.Aggregate(new List<DisEmbarkationDto>(),
			(list, item) =>
			{
				if (list.All(x => x.NationalityId != item.NationalityId))
				{
					list.Add(item);
				}
				else
				{
					list.First(x => x.NationalityId == item.NationalityId).Count += item.Count;
				}

				// total += list.Sum(x => x.Count);
				return list;
			});
		
		var vesselDocked = CrewProcessingModel!.VesselsDocked.Aggregate(new List<VesselsDockedDto>(),
			(list, item) =>
			{
				if (list.All(x => x.VesselTypeId != item.VesselTypeId))
				{
					list.Add(item);
				}
				else
				{
					list.First(x => x.VesselTypeId == item.VesselTypeId).Count += item.Count;
				}

				// total += list.Sum(x => x.Count);
				return list;
			});
		
		CrewProcessingModel.Embarkations = embarkations;
		CrewProcessingModel.DisEmbarkations = disEmbarkations;
		CrewProcessingModel.VesselsDocked = vesselDocked;
		// CrewProcessingModel.Total = total;
		
		if (CrewProcessingId == Guid.Empty)
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
		Navigation.NavigateTo("/crewprocessings");
	}
	
	protected void AddEmbarkation()
	{
		CrewProcessingModel!.Embarkations.Add(new ());
	}
	protected void AddDisEmbarkation()
	{
		CrewProcessingModel!.DisEmbarkations.Add(new ());
	}
	protected void AddVesselsDocked()
	{
		CrewProcessingModel!.VesselsDocked.Add(new ());
	}
	protected void AddAshorePass()
	{
		CrewProcessingModel!.AshorePasses.Add(new ());
	}
}