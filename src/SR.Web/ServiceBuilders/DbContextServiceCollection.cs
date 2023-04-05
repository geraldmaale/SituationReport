using Microsoft.EntityFrameworkCore;
using SR.Data;
using SR.Shared.Options;

namespace SR.Web.ServiceBuilders;

public enum DbProviders
{
    Postgres = 1,
    MssqlServer,
    MariaDb
}

public static class DbContextServiceCollection
{
    public static DbProviders DbProvider { get; set; } = DbProviders.Postgres;

    public static IServiceCollection RegisterDbContextServiceCollection(this IServiceCollection services,
        ApplicationSettings applicationSettings, IWebHostEnvironment environment)
    {
        if (DbProvider == DbProviders.Postgres)
        {
            var dbConnectionString = $"{applicationSettings?.Database!["Postgres"].DbConnection}";

            // register factory and configure the options
            if (environment.IsDevelopment())
            {
                services.AddDbContextFactory<ApplicationDbContext>(options =>
                    options.UseNpgsql(dbConnectionString)
                        .EnableSensitiveDataLogging()
                        .LogTo(Console.WriteLine, new[] {DbLoggerCategory.Database.Command.Name},
                            LogLevel.Information));
            }
            else
            {
                services.AddDbContextFactory<ApplicationDbContext>(options =>
                    options.UseNpgsql(dbConnectionString)
                        .LogTo(Console.WriteLine, new[] {DbLoggerCategory.Database.Command.Name},
                            LogLevel.Error));
            }
        }
        
        services.AddTransient<ICurrentUserService, CurrentUserService>();

        // services.AddScoped(p =>
        //     p.GetRequiredService<IDbContextFactory<ApplicationDbContext>>()
        //         .CreateDbContext());
        
        return services;
    }
}