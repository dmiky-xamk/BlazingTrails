using Ardalis.ApiEndpoints;
using BlazingTrails.Api.Persistence;
using BlazingTrails.Shared.Features.Home.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlazingTrails.Api.Features.Home.Shared;

public class GetTrailsEndpoint : BaseAsyncEndpoint
    .WithRequest<int>
    .WithResponse<GetTrailsRequest.Response>
{
    private readonly BlazingTrailsContext _database;

    public GetTrailsEndpoint(BlazingTrailsContext database)
    {
        _database = database;
    }

    [HttpGet(GetTrailsRequest.RouteTemplate)]
    public override async Task<ActionResult<GetTrailsRequest.Response>> HandleAsync(int trailId, CancellationToken cancellationToken = default)
    {
        // All trails are retrieved from the database.
        var trails = await _database.Trails
            .Include(x => x.Route)
            .ToListAsync(cancellationToken);

        foreach (var trail in trails)
        {
            Console.WriteLine(trail);
        }

        // The response is created from the list of trails.
        var response = new GetTrailsRequest
            .Response(trails.Select(trail => new GetTrailsRequest.Trail(
                trail.Id,
                trail.Name,
                trail.Image,
                trail.Location,
                trail.TimeInMinutes,
                trail.Length,
                trail.Description
                ))
            );

        return Ok(response);
    }
}
