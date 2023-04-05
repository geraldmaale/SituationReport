namespace SR.Web.ServiceBuilders;

public static class CorsServiceCollection
{
    public const String AllowOrigins = nameof(AllowOrigins);
    
    public static IServiceCollection RegisterCors(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy(AllowOrigins,
                builder =>
                {
                    builder.WithOrigins(
                            "https://localhost:7227",
                            "https://ceps.greatideasgh.org")
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });
        });
        
        return services;
    }
}