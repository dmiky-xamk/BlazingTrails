using BlazingTrails.Shared.Features.ManageTrails.AddTrail;
using MediatR;
using System.Net.Http.Json;

namespace BlazingTrails.Client.Features.ManageTrails.AddTrail;

// TRequest is the type of request the handler handles.
// TResponse is the response the handler will return.
public class AddTrailHandler :
    IRequestHandler<AddTrailRequest,
    AddTrailRequest.Response>
{
    // Inject 'IHttpClientFactory' to create a custom client.
    private readonly IHttpClientFactory _httpClientFactory;

    public AddTrailHandler(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    // Handle the request by MediatR.
    public async Task<AddTrailRequest.Response> Handle(AddTrailRequest request, CancellationToken cancellationToken)
    {
        // Create a custom HTTP client that passes access token with each request.
        var client = _httpClientFactory.CreateClient("SecureAPIClient");

        // HttpClient calls the API using the route template defined on the request.
        var response = await client.PostAsJsonAsync(
            AddTrailRequest.RouteTemplate,
            request,
            cancellationToken);

        // Read the trailId from the response and return.
        if (response.IsSuccessStatusCode)
        {
            var trailId = await response.Content.ReadFromJsonAsync<int>(
                cancellationToken: cancellationToken);

            return new AddTrailRequest.Response(trailId);
        }

        // Return a negative number to indicate a failed request.
        return new AddTrailRequest.Response(-1);
    }
}
