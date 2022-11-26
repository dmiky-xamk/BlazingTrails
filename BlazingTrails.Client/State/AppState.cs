using Blazored.LocalStorage;

namespace BlazingTrails.Client.State;

// A state store.
// A central place, where various pieces of state we want to persist can be saved and retrieved from.
public class AppState
{
    private bool _isInitialized;

    // Read-only access to states.
    public NewTrailState NewTrailState { get; }
    public FavoriteTrailsState FavoriteTrailsState { get; }

    public AppState(ILocalStorageService localStorageService)
    {
        NewTrailState = new();
        FavoriteTrailsState = new(localStorageService);
    }

    // Initialize method gives us a central place to initialize any child state stores.
    public async Task Initialize()
    {
        if (_isInitialized == false)
        {
            await FavoriteTrailsState.Initialize();
            _isInitialized = true;
        }
    }
}