using BlazingTrails.Client;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

// WebAssemblyHost = Blazor WASM
var builder = WebAssemblyHostBuilder.CreateDefault(args);

// Root components.
builder.RootComponents.Add<App>("#app"); // Entry point to the application.

// New in.NET 6.
builder.RootComponents.Add<HeadOutlet>("head::after"); // Allows to modify the head element (page title, meta tags...).

// Available to classes and components via dependency injection.
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

await builder.Build().RunAsync();
