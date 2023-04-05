#nullable disable

using Blazored.LocalStorage;
using MapsterMapper;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using Radzen;
using SR.Shared.Identity;

namespace SR.Components.Helpers
{
    public class ServiceComponentBase<TBase> : ComponentBase, IDisposable where TBase : class
    {
        [CascadingParameter(Name = "UserAccountId")]
        protected string UserAccountId { get; set; }
        [Parameter] public string UserId { get; set; }
        [Inject] public TooltipService TooltipService { get; set; }
        public bool? VisibleProperty { get; set; }
        [Inject] protected ILogger<TBase> Logger { get; set; }
        [Inject] protected IConfiguration Configuration { get; set; }
        [Inject] protected UserManager<ApplicationUser> UserManager { get; set; }
        [Inject] protected SignInManager<ApplicationUser> SignInManager { get; set; }
        [Inject]
        public ILocalStorageService BrowserStorage { get; set; }

        [Inject] public HttpClient HttpClient { get; set; }

        [Parameter] public bool ShowBreadcrumb { get; set; } = true;

        [Inject] public AuthenticationStateProvider AuthStateProvider { get; set; }

        protected bool IsLoading { get; set; } = true;
        protected bool IsDisabled { get; set; } = true;
        protected bool IsSubmitted { get; set; }

        public bool EnableInfoBar { get; set; }

        protected const string SpinnerColor = "#0288D1";

        protected const string SpinnerSize = "70px";

        [Parameter]
        public bool FilterVisible { get; set; }

        [Parameter]
        public bool DialogVisible { get; set; }

        public const string ApplicationName = "Situation Desk";

        protected IMapper MapsterMapper { get; set; } = new Mapper();


        [Inject] protected IHttpClientFactory HttpClientFactory { get; set; }

        [Inject] public IJSRuntime JsRuntime { get; set; }
        protected CancellationTokenSource CancellationTokenSource { get; set; } = new();

        [Inject] public NavigationManager Navigation { get; set; }

        public EditContext EditContext;

        public bool FormInvalid { get; set; } = true;

        [Inject] public NotificationService NotificationService { get; set; }
        [Inject] public DialogService DialogService { get; set; }


        protected void ToastSuccess(string notificationMessage)
        {
            NotificationService.Notify(NotificationSeverity.Success, notificationMessage);
        }

        protected void ToastInfo(string notificationMessage)
        {
            NotificationService.Notify(NotificationSeverity.Info, notificationMessage);
        }

        protected void ToastError(string notificationMessage)
        {
            NotificationService.Notify(NotificationSeverity.Error, notificationMessage);
        }

        protected void ToastWarning(string notificationMessage)
        {
            NotificationService.Notify(NotificationSeverity.Warning, notificationMessage);
        }

        protected void ShowTooltip(ElementReference elementReference, string content)
        {
            TooltipService.Open(elementReference, content, new TooltipOptions()
            {
                Duration = 1000
            });
        }

        // public virtual ValueTask DisposeAsync()
        // {
        //     CancellationTokenSource.Cancel();
        //     CancellationTokenSource.Dispose();
        //     return ValueTask.CompletedTask;
        // }

        public virtual void Dispose()
        {
            CancellationTokenSource.Cancel();
            CancellationTokenSource.Dispose();
            DialogService.Dispose();
        }
    }
}
