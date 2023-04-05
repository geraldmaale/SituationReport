using System.Security.Cryptography.X509Certificates;
using GreatIdeas.MailServices;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Identity.Web;
using SR.Data;
using SR.Shared.Identity;
using SR.Web.Areas.Identity;

namespace SR.Web.ServiceBuilders;

public static class IdentityServiceCollection
{
	public static IServiceCollection AddIdentityService(this IServiceCollection services)
	{
		services.AddIdentity<ApplicationUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
			.AddEntityFrameworkStores<ApplicationDbContext>()
			.AddDefaultUI()
			.AddDefaultTokenProviders()
			.AddRoles<IdentityRole>();

		services.AddScoped<AuthenticationStateProvider, RevalidatingIdentityAuthenticationStateProvider<ApplicationUser>>();

		return services;
	}

	public static IServiceCollection AddIdentityServiceWithGraph(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddIdentity<ApplicationUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
			.AddEntityFrameworkStores<ApplicationDbContext>()
			.AddDefaultUI()
			.AddDefaultTokenProviders()
			.AddRoles<IdentityRole>();

		services.AddMicrosoftIdentityWebApiAuthentication(configuration)
			.EnableTokenAcquisitionToCallDownstreamApi()
			.AddMicrosoftGraph(configuration.GetSection("DownstreamApi"))
			.AddInMemoryTokenCaches();

		services.AddScoped<AuthenticationStateProvider, RevalidatingIdentityAuthenticationStateProvider<ApplicationUser>>();

		// Azure Storage Settings
		services.Configure<AzureAdOptions>(configuration.GetSection("AzureAd"));

		// Verify certificate
		// var thumbprint = configuration.GetSection("AzureAd").GetValue<string>("Thumbprint");

		// var certificate = GetCertificate(thumbprint);
		// Console.WriteLine($"Certificate: {certificate}");

		return services;
	}

	private static X509Certificate2 GetCertificate(string thumbprint)
	{
		using var store = new X509Store(StoreName.My, StoreLocation.CurrentUser);

		store.Open(OpenFlags.ReadOnly);

		var certificateCollection = store.Certificates.Find(
			X509FindType.FindByThumbprint,
			thumbprint, true);

		if (certificateCollection.Count == 0)
		{
			throw new Exception("The specified certificate wasn't found.");
		}

		store.Close();

		return certificateCollection[0];
	}
}