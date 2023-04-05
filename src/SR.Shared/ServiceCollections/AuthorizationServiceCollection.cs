using Microsoft.Extensions.DependencyInjection;

namespace SR.Shared.ServiceCollections;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Application Policies 
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection RegisterAuthorizationPolicies(this IServiceCollection services)
    {

        services.AddAuthorizationCore(options =>
        {
        });

        return services;
    }
}