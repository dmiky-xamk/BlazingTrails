using Ardalis.ApiEndpoints;
using BlazingTrails.Api.Persistence;
using BlazingTrails.Shared.Features.ManageTrails.EditTrail;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlazingTrails.Api.Features.ManageTrails.EditTrail;

public class GetTrailEndpoint
    : BaseAsyncEndpoint
    .WithRequest<int>
    .WithResponse<GetTrailRequest.Response>
{
    private readonly BlazingTrailsContext _database;

    public GetTrailEndpoint(BlazingTrailsContext database)
    {
        _database = database;
    }

    [Authorize]
    [HttpGet(GetTrailRequest.RouteTemplate)]
    public override async Task<ActionResult<GetTrailRequest.Response>> HandleAsync(int trailId, CancellationToken cancellationToken = default)
    {
        // Load the requested trail from the database.
        var trail = await _database.Trails
            .Include(x => x.Waypoints)
            .FirstOrDefaultAsync(x => x.Id == trailId, cancellationToken: cancellationToken);

        // If the trail is not found.
        if (trail is null)
        {
            return BadRequest("Trail could not be found.");
        }

        // Only the owner can edit the trail.
        if (!trail.Owner.Equals(HttpContext.User.Identity!.Name, StringComparison.OrdinalIgnoreCase) 
            && !HttpContext.User.IsInRole("Administrator"))
        {
            return Unauthorized();
        }

        // Return an instance containing the trail's details.
        var response = new GetTrailRequest.Response(
            new GetTrailRequest.Trail(
                trail.Id,
                trail.Name,
                trail.Location,
                trail.Image,
                trail.TimeInMinutes,
                trail.Length,
                trail.Description,
                trail.Waypoints.Select(wp =>
                    new GetTrailRequest.Waypoint(wp.Latitude, wp.Longitude)
                    )
                )
            );

        return Ok(response);
    }
}