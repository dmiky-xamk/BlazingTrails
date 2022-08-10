using Ardalis.ApiEndpoints;
using BlazingTrails.Api.Persistence;
using BlazingTrails.Shared.Features.ManageTrails.EditTrail;
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

    [HttpGet(GetTrailRequest.RouteTemplate)]
    public override async Task<ActionResult<GetTrailRequest.Response>> HandleAsync(int trailId, CancellationToken cancellationToken = default)
    {
        // Load the requested trail from the database.
        var trail = await _database.Trails
            .Include(x => x.Route)
            .FirstOrDefaultAsync(x => x.Id == trailId, cancellationToken: cancellationToken);

        // If the trail is not found.
        if (trail is null)
        {
            return BadRequest("Trail could not be found.");
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
                trail.Route.Select(ri =>
                    new GetTrailRequest.RouteInstruction(ri.Id, ri.Stage, ri.Description)
                    )
                )
            );

        return Ok(response);
    }
}