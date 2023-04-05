using Microsoft.AspNetCore.Components;
using Serilog;
using SR.Components.DataServices;
using SR.Components.Helpers;
using SR.Shared;
using SR.Shared.DTOs.Crews;
using SR.Shared.DTOs.Passports;
using SR.Shared.DTOs.Permits;
using SR.Shared.DTOs.Revenues;

namespace SR.Components.Pages;

public class IndexBase : ServiceComponentBase<IndexBase>
{
	[Inject] public IRevenueCollectionDataService? RevenueCollectionDataService { get; set; }
	[Inject] public IPassportProcessingDataService? PassportProcessingDataService { get; set; }
	[Inject] public IPermitProcessingDataService? PermitProcessingDataService { get; set; }
	[Inject] public ICrewProcessingDataService? CrewProcessingDataService { get; set; }

	protected IEnumerable<RevenueCollectionDto> RevenueCollections { get; set; } = new List<RevenueCollectionDto>();
	protected IEnumerable<PassportProcessingDto> PassportProcessingData { get; set; } = new List<PassportProcessingDto>();
	protected IEnumerable<PermitProcessingDto> PermitProcessingData { get; set; } = new List<PermitProcessingDto>();
	protected IEnumerable<CrewProcessingDto> CrewProcessingData { get; set; } = new List<CrewProcessingDto>();

	protected override async Task OnAfterRenderAsync(bool firstRender)
	{
		if (firstRender)
		{
			await GetRevenues();
			await GetPassportProcessings();
			await GetPermitProcessings();
			await GetCrewProcessings();
		}
	}

	private async Task GetRevenues()
	{
		try
		{
			IsLoading = true;

			var result = await RevenueCollectionDataService!
				.GetAllAsync(CancellationTokenSource.Token);

			if (result.IsSuccessful)
			{
				RevenueCollections = result.Results.Take(7);
			}
			else
			{
				ToastError(result.Message);
			}
		}
		catch (Exception)
		{
			Log.Error(StatusLabels.ServerErrorRecords);
		}
		finally
		{
			IsLoading = false;
			StateHasChanged();
		}
	}

	private async Task GetPassportProcessings()
	{
		try
		{
			IsLoading = true;

			var result = await PassportProcessingDataService!
				.GetAllAsync(CancellationTokenSource.Token);

			if (result.IsSuccessful)
			{
				PassportProcessingData = result.Results;
			}
			else
			{
				ToastError(result.Message);
			}
		}
		catch (Exception)
		{
			Log.Error(StatusLabels.ServerErrorRecords);
		}
		finally
		{
			IsLoading = false;
			StateHasChanged();
		}
	}

	private async Task GetPermitProcessings()
	{
		try
		{
			IsLoading = true;

			var result = await PermitProcessingDataService!
				.GetAllAsync(CancellationTokenSource.Token);

			if (result.IsSuccessful)
			{
				PermitProcessingData = result.Results;
			}
			else
			{
				ToastError(result.Message);
			}
		}
		catch (Exception)
		{
			Log.Error(StatusLabels.ServerErrorRecords);
		}
		finally
		{
			IsLoading = false;
			StateHasChanged();
		}
	}

	private async Task GetCrewProcessings()
	{
		try
		{
			IsLoading = true;

			var result = await CrewProcessingDataService!
				.GetAllAsync(CancellationTokenSource.Token);

			if (result.IsSuccessful)
			{
				CrewProcessingData = result.Results;
			}
			else
			{
				ToastError(result.Message);
			}
		}
		catch (Exception)
		{
			Log.Error(StatusLabels.ServerErrorRecords);
		}
		finally
		{
			IsLoading = false;
			StateHasChanged();
		}
	}
}