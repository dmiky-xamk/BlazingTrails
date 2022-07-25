using FluentValidation;
using MediatR;

namespace BlazingTrails.Shared.Features.ManageTrails;

// Records are considered preferable for DTOs due to their immutability and value type qualities.
public record AddTrailRequest(TrailDto Trail) :
    IRequest<AddTrailRequest.Response>
{
    // Defines the address of the API endpoint for the request.
    public const string RouteTemplate = "/api/trails";

    // Defines the response data for the request.
    public record Response(int TrailId);
}

// Validator for the request. Will be executed by the API to make sure the request data is valid.
public class AddTrailRequestValidator :
    AbstractValidator<AddTrailRequest>
{
    // Reuse the validation rules created earlier.
    public AddTrailRequestValidator()
    {
        RuleFor(x => x.Trail)
            .SetValidator(new TrailValidator());
    }
}