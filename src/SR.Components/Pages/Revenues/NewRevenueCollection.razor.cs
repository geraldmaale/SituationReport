using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Radzen;
using SR.Components.DataServices;
using SR.Components.Helpers;
using SR.Shared;
using SR.Shared.DTOs.Revenues;
using SR.Shared.Entities;

namespace SR.Components.Pages.Revenues;

public class RevenueCollectionsNewBase : ServiceComponentBase<RevenueCollectionsNewBase>
{
	[Parameter] public Guid RevenueCollectionId { get; set; }
	protected string? OfficerName { get; set; }
	protected RevenueCollectionManipulationDto? RevenueCollectionModel { get; set; } = new();
	protected IEnumerable<RevenueType>? RevenueTypes { get; set; } = new List<RevenueType>();
	[Inject] public IRevenueCollectionDataService? RevenueCollectionDataService { get; set; }
	[Inject] public IRevenueTypeDataService? RevenueTypeDataService { get; set; }
    
	protected override async Task OnAfterRenderAsync(bool firstRender)
	{
		if (firstRender)
		{
			try
			{
				IsLoading = true;
				
				await RevenuesLookup();
				
				if (RevenueCollectionId != Guid.Empty)
				{
					await OnGetData();
				}
				else
				{
					// officerId
					var authenticationState = await AuthStateProvider.GetAuthenticationStateAsync();
					var user = authenticationState.User;
					var officerId = user.Claims.FirstOrDefault(c => c.Type == "OfficerId")?.Value;
					RevenueCollectionModel!.OfficerId = Guid.Parse(officerId!);
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
	
	private async Task RevenuesLookup()
	{
		try
		{
			IsLoading = true;
            
			var result = await RevenueTypeDataService!
				.GetAllAsync(CancellationTokenSource.Token);

			if (result.IsSuccessful)
			{
				RevenueTypes = result.Results;
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


	private async Task OnGetData()
	{
		var result = await RevenueCollectionDataService!
			.GetByIdAsync(RevenueCollectionId, CancellationTokenSource.Token);

		if (result.IsSuccessful)
		{
			MapsterMapper.Map(result.Result, RevenueCollectionModel);
			OfficerName = result.Result.Officer.FullName;
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
				await RevenueCollectionDataService!.CreateAsync(RevenueCollectionModel!, CancellationTokenSource.Token);
			if (result.IsSuccessful)
			{
				ToastSuccess(result.Message);
				RevenueCollectionModel = new();
				Navigation.NavigateTo("/revenuecollections");
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
			
			var result = await RevenueCollectionDataService!.UpdateAsync(
				RevenueCollectionId, RevenueCollectionModel!, CancellationTokenSource.Token);
			if (result.IsSuccessful)
			{
				ToastSuccess(result.Message);
				Navigation.NavigateTo("/revenuecollections");
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

	protected async Task OnDeleteRevenueTypeAsync(RevenueCollectionDetailDto item)
	{
		try
		{
			var dialogResult = await DialogService.Confirm($"Do you want to Revenue type data?", "Revenue Collection",
				new ConfirmOptions() {OkButtonText = "Yes", CancelButtonText = "No"});

			if (dialogResult != null && dialogResult.Value)
			{
				RevenueCollectionModel!.RevenueCollectionDetails.Remove(item);
				await RevenueCollectionDataService!.DeleteDetailAsync(item.RevenueCollectionDetailId, RevenueCollectionId, CancellationTokenSource.Token);
			}
		}
		catch (Exception)
		{
			Logger.LogError(StatusLabels.UnprocessableError);
		}
	}

	protected async Task OnValidSubmit()
	{
		// Validate and aggregate duplicated data
		var embarkations = RevenueCollectionModel!.RevenueCollectionDetails.Aggregate(new List<RevenueCollectionDetailDto>(),
			(list, item) =>
			{
				if (list.All(x => x.RevenueTypeId != item.RevenueTypeId))
				{
					list.Add(item);
				}
				else
				{
					list.First(x => x.RevenueTypeId == item.RevenueTypeId).Amount += item.Amount;
				}

				return list;
			});
		
		RevenueCollectionModel.RevenueCollectionDetails = embarkations;
		
		if (RevenueCollectionId == Guid.Empty)
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
		Navigation.NavigateTo("/revenuecollections");
	}
	
	protected void AddRevenueRecord()
	{
		RevenueCollectionModel!.RevenueCollectionDetails.Add(new ());
	}
}