using BlazingTrails.Api;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureApiServices(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // This middleware enables debugging of Blazor WebAssembly code.
    app.UseWebAssemblyDebugging();
}

app.UseHttpsRedirection();

// This middleware enables the API to serve the Blazor application.
app.UseBlazorFrameworkFiles();

// This middleware enables static files to be server by the API.
app.UseStaticFiles();

app.UseAuthorization();

app.MapControllers();

// If a request doesn't match a controller, serve the index.html file from the Blazor project.
app.MapFallbackToFile("index.html");

app.Run();
