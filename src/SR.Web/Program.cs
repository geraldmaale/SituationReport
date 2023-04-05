using GreatIdeas.Logging;
using GreatIdeas.MailServices;
using Microsoft.AspNetCore.HttpOverrides;
using Serilog;
using SR.Components;
using SR.Components.Helpers;
using SR.Data;
using SR.Data.Seed;
using SR.Shared.Options;
using SR.Shared.ServiceCollections;
using SR.Web.ServiceBuilders;

// Serilog Boostrap
LoggingServiceCollection.AddSerilogBootstrapLogging();

try
{
	var builder = WebApplication.CreateBuilder(args);

	// Add logging
	//builder.AddLoggingServices(useApplicationInsights: builder.Environment.IsDevelopment());
	
	builder.AddLoggingServices(options =>
	{
		// options.UseSeq = true;
	});

	// Register application settings
	var applicationSettings =
		builder.Configuration.GetSection(ApplicationSettings.SettingsName).Get<ApplicationSettings>();

	builder.Services.Configure<ForwardedHeadersOptions>(options =>
	{
		options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
	});

	builder.Services
		.RegisterBlazorServices()
		.RegisterDbContextServiceCollection(applicationSettings, builder.Environment)
		.AddHttpClient()
		.AddIdentityServiceWithGraph(builder.Configuration)
		.RegisterDataServices()
		.RegisterMappingServices()
		.RegisterRepositories()
		.RegisterCors()
		.AddExportFile();

	// Send emails
	builder.Services.AddTransient<IEmailSender, EmailSender>();
	builder.Services.AddMsGraphMailService(builder.Configuration);

	var app = builder.Build();

	// Configure the HTTP request pipeline.
	if (app.Environment.IsDevelopment())
	{
		app.UseMigrationsEndPoint();
	}
	else
	{
		app.UseExceptionHandler("/Error");
		// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
		app.UseHsts();
	}

	// Seed data
	SeedAuthData.Seed(app);
	SeedUsers.Users(app);
	SeedData.Seed(app);

	app.UseHttpsRedirection();

	app.UseStaticFiles();

	app.UseRouting();

	// Use logging service middleware
	app.UseSerilogCustomLoggingMiddleware();

	// Cors
	app.UseCors(CorsServiceCollection.AllowOrigins);

	app.UseAuthentication();
	app.UseAuthorization();

	app.MapControllers();
	app.MapBlazorHub();
	app.MapFallbackToPage("/_Host");

	app.Run();
}
catch (Exception ex) when (ex.GetType().Name is not "StopTheHostException")
{
	Log.Fatal(ex, "Unhandled exception");
}
finally
{
	Log.Information("Shut down complete");
	Log.CloseAndFlush();
}

return 0;