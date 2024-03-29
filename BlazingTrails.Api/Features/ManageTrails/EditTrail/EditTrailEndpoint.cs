﻿using Ardalis.ApiEndpoints;
using BlazingTrails.Api.Persistence;
using BlazingTrails.Api.Persistence.Entities;
using BlazingTrails.Shared.Features.ManageTrails.EditTrail;
using BlazingTrails.Shared.Features.ManageTrails.Shared;
using Microsoft.AspNetCore.Authorization;
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

    [Authorize]
    [HttpPut(EditTrailRequest.RouteTemplate)]
    public override async Task<ActionResult<bool>> HandleAsync(EditTrailRequest request, CancellationToken cancellationToken = default)
    {
        // The trail to edit is loaded from the database.
        var trail = await _database.Trails
            .Include(x => x.Waypoints)
            .FirstOrDefaultAsync(x => x.Id == request.Trail.Id, cancellationToken: cancellationToken);

        // The trail can't be found.
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

        // Update the trail with the details from the request.
        trail.Name = request.Trail.Name;
        trail.Description = request.Trail.Description;
        trail.Location = request.Trail.Location;
        trail.TimeInMinutes = request.Trail.TimeInMinutes;
        trail.Length = request.Trail.Length;
        trail.Waypoints = request.Trail.Waypoints
            .Select(wp => new Waypoint
            {
                Latitude = wp.Latitude,
                Longitude = wp.Longitude,
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