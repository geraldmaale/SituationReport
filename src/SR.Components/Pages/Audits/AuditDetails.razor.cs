using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using SR.Components.DataServices;
using SR.Components.Helpers;
using SR.Shared;
using SR.Shared.DTOs.Audits;

namespace SR.Components.Pages.Audits
{
    public class AuditDetailsBase: ServiceComponentBase<AuditDetailsBase>
    {
        [Inject] public IAuditDataService AuditDataService { get; set; }

        public AuditDto AuditDetail { get; set; } = new();

        [Parameter] public Guid AuditId { get; set; }

        protected override async Task OnParametersSetAsync()
        {
            try
            {
                await GetData();
            }
            catch (Exception)
            {
                Logger.LogError(StatusLabels.ServerErrorRecords);
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task GetData()
        {
            var result = await AuditDataService.GetByIdAsync(AuditId, CancellationTokenSource.Token);

            if (result.IsSuccessful)
            {
                AuditDetail = result.Result;
            }
            
        }

        public void NavigateToList()
        {
            Navigation.NavigateTo("/auditlogs");
        }
    }
}