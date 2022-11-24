using MediatR;

namespace BlazingTrails.Shared.Features.ManageTrails.EditTrail;

// Single property that holds the ID of the trail to retrieve.
public record GetTrailRequest(int TrailId) :
    IRequest<GetTrailRequest.Response>
{
    public const string RouteTemplate = "/api/trails/{trailId}";

    // Return a response that contains all the data needed by the Trail form.
    public record Response(Trail Trail);

    public record Trail(
        int Id,
        string Name,
        string Location,
        string? Image,
        int TimeInMinutes,
        int Length,
        string Description,
        IEnumerable<Waypoint> Waypoints);

    public record Waypoint(decimal Latitude, decimal Longitude);
}