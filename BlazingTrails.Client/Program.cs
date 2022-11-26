using BlazingTrails.Client;
using BlazingTrails.Client.Features.Auth;
using BlazingTrails.Client.State;
using Blazored.LocalStorage;
using MediatR;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

// WebAssemblyHost = Blazor WASM
var builder = WebAssemblyHostBuilder.CreateDefault(args);

// Let MediatR to pass the requests to a correct handler.
builder.Services.AddMediatR(typeof(Program).Assembly);

// Root components.
builder.RootComponents.Add<App>("#app"); // Entry point to the application.

// New in.NET 6.
builder.RootComponents.Add<HeadOutlet>("head::after"); // Allows to modify the head element (page title, meta tags...).

// Use a custom Blazor message handler with a named HttpClient which passes access token with every request we make.
// Easy to use with endpoints that require authentication.
// Will throw an exception if access token isn't found, so don't use with unsecure endpoints.
builder.Services.AddHttpClient("SecureAPIClient", client =>
    client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
    .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

// Available to classes and components via dependency injection.
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

// Service scopes in Blazor work differently.
// Adding a service as scoped behaves equivalently as a singleton in other ASP.NET frameworks.
// Singleton here would provide one 'AppState' instance that would be shared by every user.
// Scoped provides one 'AppState' instance per user.
builder.Services.AddScoped<AppState>();

// Make accessing local storage easier.
builder.Services.AddBlazoredLocalStorage();

// Add support for authentication using OIDC (OpenID Connect) compliant identity providers.
builder.Services.AddOidcAuthentication(opt =>
{
    // Configuration for the OIDC provider should come from the settings we put in the appsettings.json.
    builder.Configuration.Bind("Auth0", opt.ProviderOptions);

    // Authentication and authorization flow should be the type of "Authorization Code".
    opt.ProviderOptions.ResponseType = "code";

    opt.ProviderOptions.AdditionalProviderParameters.Add("audience", builder.Configuration["Auth0:Audience"]);

    // Registers our class that it will be called every time a user logs in.
}).AddAccountClaimsPrincipalFactory<CustomUserFactory<RemoteUserAccount>>();

await builder.Build().RunAsync();