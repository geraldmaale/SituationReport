using GreatIdeas.Extensions.Paging;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Radzen;
using SR.Components.DataServices;
using SR.Components.Helpers;
using SR.Shared;
using SR.Shared.DTOs.Audits;
using SR.Shared.Params;

namespace SR.Components.Pages.Audits;

public class AuditsListBase : ListServiceComponentBase<AuditsListBase, AuditDto>
{

	// public List<Fields> AuditTypes  = new ()
	// {
	// 	new(){ ID =null, Text = "None"},
	// 	new(){ ID ="Create", Text = "Create"},
	// 	new (){ ID = "Delete", Text = "Delete"},
	// 	new (){ ID = "Update", Text = "Update"}
	// };

	public AuditPagingParameters PagingParams { get; set; } = new()
	{
		PageSize = 10
	};

	public Metadata Metadata { get; set; } = new();

	[Inject] public IAuditDataService AuditDataService { get; set; }
       
	protected override async Task OnInitializedAsync()
	{
		try
		{
			await OnLoadData();
		}
		catch (Exception)
		{
			Logger.LogError(StatusLabels.ServerErrorRecords);
		}
            
	}

	protected override async Task OnLoadData()
	{
		IsLoading = true;
		PagingParams.LoadData = true;

		try
		{
			var result = await AuditDataService
				.GetAllAsync(CancellationTokenSource.Token);

			if (result.IsSuccessful)
			{
				DataSource = result.Results;
				Metadata.TotalCount = result.Results.Count();
			}
			else
			{
				ToastError(result.Message);
			}

		}
		catch (Exception)
		{
			Logger.LogError(StatusLabels.ServerErrorRecords);
			PagingParams.LoadData = false;
		}
		finally
		{
			IsLoading = false;
		}
	}
	
	protected async Task LoadData(LoadDataArgs args)
	{
		IsLoading = true;
		PagingParams.LoadData = true;

		try
		{
			var result = await AuditDataService
				.GetPagedAsync(args, CancellationTokenSource.Token);

			if (result.IsSuccessful)
			{
				DataSource = result.Items;
				Metadata = result.Metadata;
			}
			else
			{
				ToastError(result.Message);
			}

		}
		catch (Exception)
		{
			Logger.LogError(StatusLabels.ServerErrorRecords);
			PagingParams.LoadData = false;
		}
		finally
		{
			IsLoading = false;
			StateHasChanged();
		}
	}
	
	protected async Task SelectedPage(int pageIndex)
	{
		ActivePageIndex = pageIndex;
		PagingParams.PageIndex = ActivePageIndex;
		await OnLoadData();
	}
	
	protected async Task SelectedPage(PagerEventArgs eventArgs)
	{
		ActivePageIndex = eventArgs.PageIndex;
		PagingParams.PageIndex = ActivePageIndex;
		await OnLoadData();
	}
        
	protected async Task SearchChanged(string searchItem)
	{
		try
		{
			if (string.IsNullOrWhiteSpace(searchItem))
			{
				PagingParams = new AuditPagingParameters();
				// await TableGrid.ReloadServerData();
				await OnLoadData();
				PagingParams.LoadData = false;
			}
			else
			{
				PagingParams.Search = searchItem;
				// await TableGrid.ReloadServerData();
				await OnLoadData();
			}
		}
		catch (Exception)
		{
			Logger.LogError("Could not search data");
		}
	}

	protected override void OnDetailData(AuditDto Audit)
	{
		try
		{
			Navigation.NavigateTo($"auditlogs/{Audit.Id}/details");
		}
		catch (Exception)
		{
			Logger.LogError(StatusLabels.UnprocessableError);
		}
	}

        
	#region Filtering

	protected void OnFilterClicked()
	{
		FilterVisible = true;
	}
        
	protected async Task FilterChanged()
	{
		await OnFilter();
	}

	private async Task OnFilter()
	{
		FilterVisible = false;
		await OnLoadData();
	}
        
	protected void FilterCleared()
	{
		PagingParams = new AuditPagingParameters();
		FilterVisible = false;
		PagingParams.LoadData = false;
	}

	#endregion
        

	protected async Task ExportAuditlogs()
	{
		IsSubmitted = true;
		try
		{
			PagingParams.FileName = $"Audit Logs {DateTime.Now:yyMMddHHmmss}";
			var result = await AuditDataService.ExportAsync(
				PagingParams, CancellationTokenSource.Token);

			if (result.IsSuccessful)
			{
				ToastSuccess(result.Message);
			}
			else
			{
				ToastError(result.Message);
			}
		}
		catch (Exception)
		{
			Logger.LogError(StatusLabels.ServerErrorRecords);
		}
		finally
		{
			IsSubmitted = false;
			StateHasChanged();
		}
	}
}