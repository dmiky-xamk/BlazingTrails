using BlazingTrails.Shared.Features.ManageTrails.Shared;

namespace BlazingTrails.Client.State;

// A state store for creating a new trail.
public class NewTrailState
{
    // Store unsaved trail data.
    // Private - generally state shouldn't be altered directly.
    private TrailDto _unsavedNewTrail = new();

    // Methods to manipulate the unsaved trail with.
    public TrailDto GetTrail() => _unsavedNewTrail;
    public void SaveTrail(TrailDto trail) => _unsavedNewTrail = trail;
    public void ClearTrail() => _unsavedNewTrail = new();
}
