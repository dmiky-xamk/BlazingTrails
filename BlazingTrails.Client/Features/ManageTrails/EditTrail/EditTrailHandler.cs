﻿using BlazingTrails.Shared.Features.ManageTrails.EditTrail;
using MediatR;
using System.Net.Http.Json;

namespace BlazingTrails.Client.Features.ManageTrails.EditTrail;

public class EditTrailHandler : IRequestHandler<EditTrailRequest, EditTrailRequest.Response>
{
    private readonly IHttpClientFactory _httpClientFactory;

    public EditTrailHandler(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<EditTrailRequest.Response> Handle(EditTrailRequest request, CancellationToken cancellationToken)
    {
        // Create a custom HTTP client that passes access token with each request.
        var client = _httpClientFactory.CreateClient("SecureAPIClient");

        // The updated trail details are sent to the API via a HTTP PUT request.
        var response = await client.PutAsJsonAsync(EditTrailRequest.RouteTemplate, request, cancellationToken);

        if (response.IsSuccessStatusCode)
        {
            return new EditTrailRequest.Response(true);
        }

        else
        {
            return new EditTrailRequest.Response(false);
        }
    }
}