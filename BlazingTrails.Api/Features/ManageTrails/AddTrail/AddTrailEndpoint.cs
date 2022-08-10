using Ardalis.ApiEndpoints;
using BlazingTrails.Api.Persistence;
using BlazingTrails.Api.Persistence.Entities;
using BlazingTrails.Shared.Features.ManageTrails.AddTrail;
using Microsoft.AspNetCore.Mvc;

namespace BlazingTrails.Api.Features.ManageTrails.AddTrail;

/*
 * QUESTION:
 * Does the user have access to the Validator located inside the BlazingTrails.Shared so that they can modify its functionality?
*/

// Using the ApiEndpoints library.
// Defining the request and the response the endpoint will handle.
public class AddTrailEndpoint : BaseAsyncEndpoint
    .WithRequest<AddTrailRequest>
    .WithResponse<int>
{
    private readonly BlazingTrailsContext _database;

    public AddTrailEndpoint(BlazingTrailsContext database)
    {
        _database = database;
    }

    // The route to the endpoint is defined using the template on the Request.
    [HttpPost(AddTrailRequest.RouteTemplate)]
    public override async Task<ActionResult<int>> HandleAsync(AddTrailRequest request, CancellationToken cancellationToken = default)
    {
        // Create a new instance using the data in the request.
        // This is the entity that will be persisted to the database.
        var trail = new Trail
        {
            Name = request.Trail.Name,
            Description = request.Trail.Description,
            Location = request.Trail.Location,
            TimeInMinutes = request.Trail.TimeInMinutes,
            Length = request.Trail.Length
        };

        await _database.Trails.AddAsync(trail, cancellationToken);

        // A collection of RouteInstructions is created using the data in the request.
        // These will be persisted to the database.
        var routeInstructions = request.Trail.Route
            .Select(x => new RouteInstruction
            {
                Stage = x.Stage,
                Description = x.Description,
                Trail = trail
            });

        await _database.RouteInstructions.AddRangeAsync(routeInstructions, cancellationToken);


        // Persist the changes.
        await _database.SaveChangesAsync(cancellationToken);

        // Return the newly created trail ID.
        return Ok(trail.Id);
    }
}