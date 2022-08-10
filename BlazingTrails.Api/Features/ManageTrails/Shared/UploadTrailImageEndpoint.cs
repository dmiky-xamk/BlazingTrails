using Ardalis.ApiEndpoints;
using BlazingTrails.Api.Persistence;
using BlazingTrails.Shared.Features.ManageTrails.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace BlazingTrails.Api.Features.ManageTrails.Shared;

public class UploadTrailImageEndpoint :
    BaseAsyncEndpoint.WithRequest<int>
    .WithResponse<string>
{
    private readonly BlazingTrailsContext _database;

    public UploadTrailImageEndpoint(BlazingTrailsContext database)
    {
        _database = database;
    }

    [HttpPost(UploadTrailImageRequest.RouteTemplate)]
    public override async Task<ActionResult<string>> HandleAsync([FromRoute] int trailId, CancellationToken cancellationToken = default)
    {
        // Load the trail and return 400 if it doesn't exist.
        var trail = await _database.Trails.FirstOrDefaultAsync(x => x.Id == trailId, cancellationToken);

        if (trail is null)
        {
            return BadRequest("Trail does not exist.");
        }

        // Attempt to load the file posted in the request and return 400 if it can't be found.
        // Request object is available in every endpoint and allows access to information regarding the current HTTP request.
        var file = Request.Form.Files[0];

        if (file.Length == 0)
        {
            return BadRequest("No image found.");
        }

        // Create a new filename that is safe to use in the application.
        var filename = $"{Guid.NewGuid()}.jpg";

        // Specify the save location as the newly created Images folder.
        // Could be saved to a cloud provider etc.
        var saveLocation = Path.Combine(Directory.GetCurrentDirectory(), "Images", filename);

        // ImageSharp resizes the image and saves it to the filesystem.
        // The image keeps it original aspect ratio when using ImageSharp.
        var resizeOptions = new ResizeOptions
        {
            Mode = ResizeMode.Pad,
            Size = new Size(640, 426)
        };

        using var image = Image.Load(file.OpenReadStream());
        image.Mutate(x => x.Resize(resizeOptions));

        await image.SaveAsJpegAsync(saveLocation, cancellationToken);

        // To update a trail image, we need to remove the existing image if present.
        if (string.IsNullOrWhiteSpace(trail.Image) == false)
        {
            // Remove the old image from the filesystem.
            System.IO.File.Delete(Path.Combine(Directory.GetCurrentDirectory(), "Images", trail.Image));
        }

        // Updat the trail with the location of the image. Will be used to update the UI.
        trail.Image = filename;
        await _database.SaveChangesAsync(cancellationToken);

        return Ok(trail.Image);
    }
}
