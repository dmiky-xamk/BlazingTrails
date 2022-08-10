﻿using Ardalis.ApiEndpoints;
using BlazingTrails.Api.Persistence;
using BlazingTrails.Api.Persistence.Entities;
using BlazingTrails.Shared.Features.ManageTrails.EditTrail;
using BlazingTrails.Shared.Features.ManageTrails.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlazingTrails.Api.Features.ManageTrails.EditTrail;

public class EditTrailEndpoint
    : BaseAsyncEndpoint
    .WithRequest<EditTrailRequest>
    .WithResponse<bool>
{
    private readonly BlazingTrailsContext _database;

    public EditTrailEndpoint(BlazingTrailsContext database)
    {
        _database = database;
    }

    [HttpPut(EditTrailRequest.RouteTemplate)]
    public override async Task<ActionResult<bool>> HandleAsync(EditTrailRequest request, CancellationToken cancellationToken = default)
    {
        // The trail to edit is loaded from the database.
        var trail = await _database.Trails
            .Include(x => x.Route)
            .FirstOrDefaultAsync(x => x.Id == request.Trail.Id, cancellationToken: cancellationToken);

        // The trail can't be found.
        if (trail is null)
        {
            return BadRequest("Trail could not be found.");
        }

        // Update the trail with the details from the request.
        trail.Name = request.Trail.Name;
        trail.Description = request.Trail.Description;
        trail.Location = request.Trail.Location;
        trail.TimeInMinutes = request.Trail.TimeInMinutes;
        trail.Length = request.Trail.Length;
        trail.Route = request.Trail.Route.Select(ri =>
                    new RouteInstruction
                    {
                        Stage = ri.Stage,
                        Description = ri.Description,
                        Trail = trail
                    }).ToList();

        // Remove the physical file from the disk and set the Image property to null.
        if (request.Trail.ImageAction == ImageAction.Remove)
        {
            System.IO.File.Delete(Path.Combine(Directory.GetCurrentDirectory(), "Images", trail.Image!));
            trail.Image = null;
        }

        await _database.SaveChangesAsync(cancellationToken);

        return Ok(true);
    }
}