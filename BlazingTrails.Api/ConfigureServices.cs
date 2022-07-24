using BlazingTrails.Api.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BlazingTrails.Api;

public static class ConfigureServices
{
    public static IServiceCollection ConfigureApiServices(this IServiceCollection services, ConfigurationManager config)
    {
        // Register the context and specify the database we're using.
        services.AddDbContext<BlazingTrailsContext>(opt =>
            opt.UseSqlite(config.GetConnectionString("BlazingTrailsContext")));

        services.AddControllers();

        return services;
    }
}
