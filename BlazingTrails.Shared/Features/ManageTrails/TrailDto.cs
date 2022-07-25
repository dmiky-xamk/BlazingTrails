﻿using FluentValidation;

namespace BlazingTrails.Shared.Features.ManageTrails;

public class TrailDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public int TimeInMinutes { get; set; }
    public int Length { get; set; }
    public List<RouteInstruction> Route { get; set; } = new List<RouteInstruction>();

    public class RouteInstruction
    {
        public int Stage { get; set; }
        public string Description { get; set; } = string.Empty;
    }
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