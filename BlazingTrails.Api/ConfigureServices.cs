using BlazingTrails.Api.Persistence;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using System.Security.Claims;

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

        // Add services required by authentication services and allow to configure options.
        services.AddAuthentication(opt =>
        {
            opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            // Use JSON Web Tokens.
        }).AddJwtBearer(opt =>
        {
            // Where to validate the tokens.
            opt.Authority = config["Auth0:Domain"];
            opt.Audience = config["Auth0:Audience"];
        });

        return services;
    }
}
