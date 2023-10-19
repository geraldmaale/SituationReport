using Blazored.LocalStorage;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.ResponseCompression;
using OfficeOpenXml;
using Radzen;
using SR.Shared;
using Syncfusion.Licensing;

namespace SR.Web.ServiceBuilders
{
    public static class BlazorClientCollection
    {
        public static IServiceCollection RegisterBlazorServices(this IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddServerSideBlazor()
                .AddCircuitOptions(options => { options.DetailedErrors = true; })
                .AddHubOptions(options =>
                {
                    options.MaximumReceiveMessageSize = 50 * 1024 * 1024; // 50MB
                });

            //FluentValidation
            services.AddFluentValidation(configuration => 
	            configuration.RegisterValidatorsFromAssemblyContaining<SharedAssemblyMarker>());

            services.AddResponseCompression(opts =>
            {
                opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
                    new[] { "application/octet-stream" });
            });

            // Route Options
            services.Configure<RouteOptions>(options =>
            {
                // options.LowercaseUrls = true;
                // options.LowercaseQueryStrings = true;
            });

            // Syncfusion Backend
            SyncfusionLicenseProvider.RegisterLicense("<license key here ...>");

            // EPPlus
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            // SignalR
            services.AddSignalR();

            services.AddBlazoredLocalStorage();
            // services.AddBlazoredSessionStorage();

            // Radzen Services
            services.AddScoped<DialogService>();
            services.AddScoped<NotificationService>();
            services.AddScoped<TooltipService>();
            services.AddScoped<ContextMenuService>();

            // MudBlazor
			// services.AddMudServices();

            return services;
        }

    }
}
