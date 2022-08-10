using FluentValidation;

namespace BlazingTrails.Shared.Features.ManageTrails.Shared;

public class TrailDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public int TimeInMinutes { get; set; }
    public int Length { get; set; }
    public List<RouteInstruction> Route { get; set; } = new List<RouteInstruction>();

    // Holds the filename of an existing image.
    public string? Image { get; set; }

    // Allows us to set what operation to perform on the trail image when updating the trail.
    public ImageAction ImageAction { get; set; }

    public class RouteInstruction
    {
        public int Stage { get; set; }
        public string Description { get; set; } = string.Empty;
    }
}

// Containts the various operations that can be performed on an image.
public enum ImageAction
{
    None,
    Add,
    Remove
}

// Create a validator to validate the user inputs.
public class TrailValidator : AbstractValidator<TrailDto>
{
    // Validation rules are defined in the constructor.
    public TrailValidator()
    {
        RuleFor(x => x.Name).NotEmpty()
            .WithMessage("Please enter a name");

        RuleFor(x => x.Description).NotEmpty()
            .WithMessage("Please enter a description");

        RuleFor(x => x.Location).NotEmpty()
            .WithMessage("Please enter a location");

        RuleFor(x => x.Length).GreaterThan(0)
            .WithMessage("Please enter a length");

        RuleFor(x => x.Route).NotEmpty()
            .WithMessage("Please add a route instruction");

        RuleFor(x => x.TimeInMinutes).GreaterThan(0)
            .WithMessage("Please enter a time");

        // Validate each entry in the Route collection by using the given validator.
        RuleForEach(x => x.Route).SetValidator(new RouteInstructionValidator());
    }
}

public class RouteInstructionValidator : AbstractValidator<TrailDto.RouteInstruction>
{
    public RouteInstructionValidator()
    {
        RuleFor(x => x.Stage).NotEmpty()
            .WithMessage("Please enter a stage");

        RuleFor(x => x.Description).NotEmpty()
            .WithMessage("Please enter a description");
    }
}