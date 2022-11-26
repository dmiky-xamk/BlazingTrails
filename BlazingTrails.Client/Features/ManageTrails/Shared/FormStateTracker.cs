using BlazingTrails.Client.State;
using BlazingTrails.Shared.Features.ManageTrails.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace BlazingTrails.Client.Features.ManageTrails.Shared;

// Mark a class as a component using 'ComponentBase'.
public class FormStateTracker : ComponentBase
{
    // Inject 'AppState' (service) into the component.
    // Blazor uses property injection, so we decorate the property with 'Inject' attribute.
    [Inject]
    public AppState AppState { get; set; }

    // Capture a reference to the 'EditContext', which is cascaded from the 'EditForm' component.
    // Allow us to know when the model is updated and take a copy.
    [CascadingParameter]
    private EditContext CascadedEditContext { get; set; }

    protected override void OnInitialized()
    {
        // Sanity check to make sure the component is used within an 'EditForm'.
        if (CascadedEditContext is null)
        {
            throw new InvalidOperationException($"{nameof(FormStateTracker)} required a cascading parameter of type {nameof(EditContext)}");
        }

        // Subscribe to 'OnFieldChanged' event to be notified every time the model is updated.
        CascadedEditContext.OnFieldChanged += CascadedEditContext_OnFieldChanged;
    }

    private void CascadedEditContext_OnFieldChanged(object? sender, FieldChangedEventArgs e)
    {
        // When the event fires we grab a copy of the model.
        var trail = (TrailDto)e.FieldIdentifier.Model;

        // Only new trails have an ID of 0, so if this is the case, we savae the model to our state store.
        if (trail.Id == 0)
        {
            AppState.NewTrailState.SaveTrail(trail);
        }
    }
}
