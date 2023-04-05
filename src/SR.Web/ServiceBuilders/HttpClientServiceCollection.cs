using System.Net.Http.Headers;
using SR.Shared.Options;

namespace SR.Web.ServiceBuilders
{
    public static class HttpClientServiceCollection
    {
        public static IServiceCollection RegisterHttpClientServices(this IServiceCollection services,
            ApplicationSettings applicationSettings)
        {

            string uriConnection = $"{applicationSettings?.UriConnection}";

            services.AddScoped(sp => new HttpClient
            {
                BaseAddress = new Uri(uriConnection)
            });

            services.AddHttpClient("API", c =>
            {
                c.BaseAddress = new Uri(uriConnection);
                c.DefaultRequestHeaders.Clear();
                c.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));
            });

            return services;
        }
    }
}