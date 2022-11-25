using BlazingTrails.Shared.Features.ManageTrails.Shared;
using MediatR;
using System.Net.Http;

namespace BlazingTrails.Client.Features.ManageTrails.Shared;

public class UploadTrailImageHandler :
    IRequestHandler<UploadTrailImageRequest,
        UploadTrailImageRequest.Response>
{
    private readonly IHttpClientFactory _httpClientFactory;

    public UploadTrailImageHandler(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<UploadTrailImageRequest.Response> Handle(UploadTrailImageRequest request, CancellationToken cancellationToken)
    {

        // Read the file as a stream.
        var fileContent = request.File.OpenReadStream(request.File.Size, cancellationToken);

        // Add the file to data content.
        using var content = new MultipartFormDataContent
        {
            { new StreamContent(fileContent), "image", request.File.Name }
        };

        // Create a custom HTTP client that passes access token with each request.
        var client = _httpClientFactory.CreateClient("SecureAPIClient");

        // Post the file to the API.
        // GIVE THE FILE A NEW NAME IN THE API! DO NOT USE THE USER GIVEN NAME AS IT COULD BE MALICIOUS!
        var response = await client.PostAsync(UploadTrailImageRequest.RouteTemplate.Replace("{trailId}", request.TrailId.ToString()), content, cancellationToken);

        // Deserialize and return a successful API response.
        if (response.IsSuccessStatusCode)
        {
            var filename = await response.Content.ReadAsStringAsync(cancellationToken: cancellationToken);

            return new UploadTrailImageRequest.Response(filename);
        }

        // Return an emptry string if the upload failed.
        else
        {
            return new UploadTrailImageRequest.Response("");
        }
    }
}
