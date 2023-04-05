using Microsoft.AspNetCore.Components;
using Serilog;
using SR.Components.DataServices;
using SR.Components.Helpers;
using SR.Shared;
using SR.Shared.Params;

namespace SR.Components.Pages
{
	public class ReportsBase : ServiceComponentBase<ReportsBase>
	{
		public PagingParameters PagingParameters { get; set; } = new();

		[Inject] public IReportsDataService? ReportsDataService { get; set; }

		protected override void OnInitialized()
		{
			// Get beginning of the week
			var endOfWeek = DateTime.UtcNow;

			// Get end of the week
			var beginningOfWeek = DateTime.UtcNow.Subtract(TimeSpan.FromDays(6));
			PagingParameters.StartDate = beginningOfWeek.Date;
			PagingParameters.EndDate = endOfWeek.Date;
		}

		protected async Task OnExportRevenues()
		{
			try
			{
				IsSubmitted = true;
				var startDateLabel = PagingParameters.StartDate!.Value.ToString(DateLabels.ShortDateFormat);
				var endDateLabel = PagingParameters.EndDate!.Value.ToString(DateLabels.ShortDateFormat);

				PagingParameters.Title = $"Revenue Collections for {startDateLabel}-{endDateLabel}";
				PagingParameters.ExportType = $"Revenue Collections for {startDateLabel}-{endDateLabel}";
				PagingParameters.FileName = $"Revenues-{PagingParameters.EndDate.Value.ToString("dd-MM-yy")}";

				var results = await ReportsDataService!.ExportRevenues(PagingParameters, CancellationTokenSource.Token);
				if (results.IsSuccessful)
				{
					ToastSuccess(results.Message);
				}
				else
				{
					ToastError(results.Message);
				}
			}
			catch (Exception)
			{
				Log.Information(StatusLabels.UnprocessableError);
			}
			finally
			{
				IsSubmitted = false;
				StateHasChanged();
			}
		}
	}
}