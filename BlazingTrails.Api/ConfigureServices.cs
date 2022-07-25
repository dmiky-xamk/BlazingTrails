using BlazingTrails.Api.Persistence;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace BlazingTrails.Api;

public static class ConfigureServices
{
    public static IServiceCollection ConfigureApiServices(this IServiceCollection services, ConfigurationManager config)
    {
        // Register the context and specify the database we're using.
        services.AddDbContext<BlazingTrailsContext>(opt =>
            opt.UseSqlite(config.GetConnectionString("BlazingTrailsContext")));

        // Register all the validators from the assembly.
        services.AddControllers()
            .AddFluentValidation(fv =>
                fv.RegisterValidatorsFromAssembly(Assembly.Load("BlazingTrails.Shared")));

        return services;
    }
}
