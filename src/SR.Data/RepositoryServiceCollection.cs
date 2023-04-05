using Microsoft.Extensions.DependencyInjection;
using SR.Data.Repositories.Contracts;
using SR.Data.Repositories.Persistence;

namespace SR.Data;

public static class RepositoryServiceCollection
{
    public static IServiceCollection RegisterRepositories(this IServiceCollection services)
    {
	    services.AddScoped<IPermitTypeRepository, PermitTypeRepository>();
        services.AddScoped<IPermitTypeRepo, PermitTypeRepo>();
        services.AddScoped<IAshorePassTypeRepository, AshorePassTypeRepository>();
        services.AddScoped<INationalityTypeRepository, NationalityTypeRepository>();
        services.AddScoped<IOperationTypeRepository, OperationTypeRepository>();
        services.AddScoped<IRevenueTypeRepository, RevenueTypeRepository>();
        services.AddScoped<IVesselTypeRepository, VesselTypeRepository>();

        services.AddScoped<IPermitProcessingRepository, PermitProcessingRepository>();
        services.AddScoped<IPermitProcessingDetailRepository, PermitProcessingDetailRepository>();
		services.AddScoped<IOfficerRepository, OfficerRepository>();
		services.AddScoped<IAuditRepository, AuditRepository>();
		services.AddScoped<ICrewProcessingRepository, CrewProcessingRepository>();
		services.AddScoped<IPassportProcessingRepository, PassportProcessingRepository>();
		services.AddScoped<IRevenueRepository, RevenueRepository>();
		services.AddScoped<IOperationOfficeRepository, OperationOfficeRepository>();
		services.AddScoped<IOperationProcessingRepository, OperationProcessingRepository>();
		services.AddScoped<IUserRepository, UserRepository>();
        
        return services;
    }
}