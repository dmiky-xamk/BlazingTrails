using MediatR;
using Microsoft.AspNetCore.Components.Forms;

namespace BlazingTrails.Shared.Features.ManageTrails.Shared;

public record UploadTrailImageRequest(int TrailId, IBrowserFile File)
    : IRequest<UploadTrailImageRequest.Response>
{
    // Route for the request.
    public const string RouteTemplate = "/api/trails/{trailId}/images";

    // The response the request will return.
    public record Response(string ImageName);
}