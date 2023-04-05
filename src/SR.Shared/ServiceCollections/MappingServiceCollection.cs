using System.Reflection;
using Mapster;
using Microsoft.Extensions.DependencyInjection;
using SR.Shared.Mappers;

namespace SR.Shared.ServiceCollections;

public static class MappingServiceCollection
{
    public static IServiceCollection RegisterMappingServices(this IServiceCollection services)
    {
        // Mapster Mapping
        TypeAdapterConfig.GlobalSettings.Scan(Assembly.GetAssembly(typeof(SharedAssemblyMarker))!);

        return services;
    }
}