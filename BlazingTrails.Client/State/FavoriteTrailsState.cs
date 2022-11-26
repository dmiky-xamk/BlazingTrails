using BlazingTrails.Client.Features.Shared;
using Blazored.LocalStorage;

namespace BlazingTrails.Client.State;

public class FavoriteTrailsState
{
    private const string _favoriteTrailsKey = "favoriteTrails";
    private bool _isInitialized;
    private readonly ILocalStorageService _localStorageService;

    // Current favorited trails are kept in a private list so they can't be manipulated directly.
    private List<Trail> _favoriteTrails = new();

    // The favorite trails are exposed via a read-only list.
    public IReadOnlyList<Trail> FavoriteTrails => _favoriteTrails.AsReadOnly();

    // 'OnChange' event allows concerned components to subscribe to changes in the store.
    public event Action? OnChange;

    // Gives us the ability to read and write to the browser's local storage feature.
    public FavoriteTrailsState(ILocalStorageService localStorageService)
    {
        _localStorageService = localStorageService;
    }

    // Will be called when the application initially boots in order to set up the store.
    public async Task Initialize()
    {
        if (_isInitialized == false)
        {
            _favoriteTrails = await _localStorageService.GetItemAsync<List<Trail>>(_favoriteTrailsKey) ?? new List<Trail>();
            _isInitialized = true;

            NotifyHasChanged();
        }
    }

    // Add a trail to favorites and persist a copy to local storage.
    public async Task AddFavorite(Trail trail)
    {
        if (_favoriteTrails.Any(x => x.Id == trail.Id))
        {
            return;
        }

        _favoriteTrails.Add(trail);

        await _localStorageService.SetItemAsync(_favoriteTrailsKey, _favoriteTrails);

        NotifyHasChanged();
    }

    // Remove a trail from favorites and save the update to local storage.
    public async Task RemoveFavorite(Trail trail)
    {
        var existingTrail = _favoriteTrails.FirstOrDefault(x => x.Id == trail.Id);

        if (existingTrail is null)
        {
            return;
        }

        _favoriteTrails.Remove(existingTrail);

        await _localStorageService.SetItemAsync(_favoriteTrailsKey, _favoriteTrails);

        NotifyHasChanged();
    }

    public bool IsFavorite(Trail trail) => _favoriteTrails.Any(x => x.Id == trail.Id);

    // Raise the 'OnChange' event, which notifies subscribers that something has changed in the store.
    private void NotifyHasChanged() => OnChange?.Invoke();
}
