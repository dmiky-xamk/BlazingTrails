using Microsoft.AspNetCore.Components.Forms;

namespace BlazingTrails.Client.Validation;

// Override the Blazor valid and invalid input state classes with Bootstraps.
public class BootstrapCssClassProvider : FieldCssClassProvider
{
    // EditContext: The brain of the form and keeps track of each field in the form.
    // FieldIdentifier: The field in the form we're getting CSS classes for.
    public override string GetFieldCssClass(EditContext editContext, in FieldIdentifier fieldIdentifier)
    {
        // Check if the current field has any validation errors and set isValid appropriately.
        var isValid = !editContext
            .GetValidationMessages(fieldIdentifier).Any();

        // The field has been modified. Return custom CSS classes depending on whether the field is valid or not.
        if (editContext.IsModified(fieldIdentifier))
        {
            return isValid ? " is-valid" : "is-invalid";
        }

        // The field has not been modified. Return a custom CSS class if the field is invalid but not if it's valid.
        return isValid ? string.Empty : "is-invalid";
    }
}
