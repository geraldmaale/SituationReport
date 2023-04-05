
using SR.Data;
using SR.Web.Helpers;

namespace SR.Web.ServiceBuilders;

public static class ExportFileServiceCollection
{
    public static IServiceCollection AddExportFile(this IServiceCollection services)
    {
        services.AddTransient<ExportFileHelper>();
        return services;
    }
}