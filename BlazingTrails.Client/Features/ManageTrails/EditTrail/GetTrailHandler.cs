using BlazingTrails.Shared.Features.ManageTrails.EditTrail;
using MediatR;
using System.Net.Http.Json;

namespace BlazingTrails.Client.Features.ManageTrails.EditTrail;

public class GetTrailHandler : IRequestHandler<GetTrailRequest, GetTrailRequest.Response?>
{
    private readonly IHttpClientFactory _httpClientFactory;

    public GetTrailHandler(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<GetTrailRequest.Response?> Handle(GetTrailRequest request, CancellationToken cancellationToken)
    {
        try
        {
            // Create a custom HTTP client that passes access token with each request.
            var client = _httpClientFactory.CreateClient("SecureAPIClient");

            // Replace the trailId of the trail to edit before making the HTTP request.
            return await client.GetFromJsonAsync<GetTrailRequest.Response>(
                GetTrailRequest.RouteTemplate
                .Replace("{trailId}", request.TrailId.ToString()));
        }

        catch (HttpRequestException)
        {
            return default!;
        }
    }
}
