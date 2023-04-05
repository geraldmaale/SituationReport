using Microsoft.Extensions.DependencyInjection;
using SR.Components.DataServices;

namespace SR.Components
{
	public static class DataServiceCollection
	{
		public static IServiceCollection RegisterDataServices(this IServiceCollection services)
		{
			services.AddScoped<IPermitTypeDataService, PermitTypeDataService>();
			services.AddScoped<IVesselTypeDataService, VesselTypeDataService>();
			services.AddScoped<IRevenueTypeDataService, RevenueTypeDataService>();
			services.AddScoped<IOperationTypeDataService, OperationTypeDataService>();
			services.AddScoped<IAshorePassTypeDataService, AshorePassTypeDataService>();
			services.AddScoped<INationalityTypeDataService, NationalityTypeDataService>();
			services.AddScoped<IOfficerDataService, OfficerDataService>();
			services.AddScoped<IAuditDataService, AuditDataService>();
			services.AddScoped<ICrewProcessingDataService, CrewProcessingDataService>();
			services.AddScoped<IPermitProcessingDataService, PermitProcessingDataService>();
			services.AddScoped<IPassportProcessingDataService, PassportProcessingDataService>();
			services.AddScoped<IRevenueCollectionDataService, RevenueCollectionDataService>();
			services.AddScoped<IOperationOfficeDataService, OperationOfficeDataService>();
			services.AddScoped<IOperationInspectionDataService, OperationInspectionDataService>();
			services.AddScoped<IUserDataService, UserDataService>();
			services.AddScoped<IReportsDataService, ReportsDataService>();

			return services;
		}
	}
}