using System.Net.Http.Json;
using Microsoft.AspNetCore.Components.Routing;
using SmallsOnline.Web.Lib.Models.FavoritesOf.Albums;
using SmallsOnline.Web.Lib.Models.FavoritesOf.Songs;

namespace SmallsOnline.Web.PublicSite.Client;

/// <summary>
/// The page for displaying the favorite music of a given year.
/// </summary>
public partial class FavoritesOf : ComponentBase, IDisposable
{
    [Inject] protected IHttpClientFactory HttpClientFactory { get; set; } = null!;

    [Inject] protected PersistentComponentState AppState { get; set; } = null!;

    [Inject] protected ILogger<FavoritesOf> PageLogger { get; set; } = null!;

    [Inject] protected NavigationManager NavigationManager { get; set; } = null!;

    [Inject] protected FavoritesOfStateContainer StateContainer { get; set; } = null!;

    [Parameter] public string? ListYear { get; set; }

    [CascadingParameter(Name = "ShouldFadeSlideIn")]
    protected ShouldFadeIn? ShouldFadeSlideIn { get; set; }

    private bool _isFinishedLoading = false;
    private PersistingComponentStateSubscription? _persistenceSubscription;
    private List<AlbumData>? _albumItems;
    private List<SongData>? _trackItems;

    private ElementReference _favoriteAlbumsRef;
    private ElementReference _favoriteSongsRef;


    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected override async Task OnParametersSetAsync()
    {
        StateContainer.OnChange += StateHasChanged;
        NavigationManager.LocationChanged += OnLocationChange;

        if (ListYear is not null)
        {
            _persistenceSubscription = AppState.RegisterOnPersisting(PersistFavoritesOfData);

            _isFinishedLoading = false;

            if (ListYear != StateContainer.ListYear)
            {
                await GetFavorites();
                StateContainer.ListYear = ListYear;
            }

            _isFinishedLoading = true;
        }

        base.OnParametersSet();
    }

    private void OnLocationChange(object? sender, LocationChangedEventArgs eventArgs)
    {
        if (eventArgs.Location.Contains("/top-music/favorites-of/"))
        {
            StateContainer.ListYear = ListYear;
        }
        else
        {
            StateContainer.ListYear = null;
        }
    }

    /// <summary>
    /// Get the favorite music items from the API.
    /// </summary>
    private async Task GetFavorites()
    {
        PageLogger.LogInformation("Loading favorites of {ListYear}", ListYear);

        bool isAlbumItemsDataAvailable = AppState.TryTakeFromJson(
            key: $"albumItemsData-{ListYear}",
            instance: out List<AlbumData>? restoredAlbumItemsData
        );

        bool isTrackItemsDataAvailable = AppState.TryTakeFromJson(
            key: $"trackItemsData-{ListYear}",
            instance: out List<SongData>? restoredTrackItemsData
        );

        using HttpClient httpClient = HttpClientFactory.CreateClient("PublicApi");
        if (!isAlbumItemsDataAvailable)
        {
            // Get the favorite albums from the API.
            _albumItems = await httpClient.GetFromJsonAsync<List<AlbumData>?>($"api/favoriteAlbums/{ListYear}");
        }
        else
        {
            PageLogger.LogInformation(
                "Album list data was persisted from a prerendered state. Restoring that data instead.");
            _albumItems = restoredAlbumItemsData;
        }

        if (!isTrackItemsDataAvailable)
        {
            // Get the favorite tracks from the API.
            _trackItems = await httpClient.GetFromJsonAsync<List<SongData>?>($"api/favoriteTracks/{ListYear}");
        }
        else
        {
            PageLogger.LogInformation(
                "Track list data was persisted from a prerendered state. Restoring that data instead.");
            _trackItems = restoredTrackItemsData;
        }
    }

    private Task PersistFavoritesOfData()
    {
        AppState.PersistAsJson(
            key: $"albumItemsData-{ListYear}",
            instance: _albumItems
        );

        AppState.PersistAsJson(
            key: $"trackItemsData-{ListYear}",
            instance: _trackItems
        );

        return Task.CompletedTask;
    }

    private async Task ScrollToFavoriteAlbums()
    {
        await _favoriteAlbumsRef.FocusAsync();
    }

    private async Task ScrollToFavoriteTracks()
    {
        await _favoriteSongsRef.FocusAsync();
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            if (_persistenceSubscription.HasValue)
            {
                _persistenceSubscription.Value.Dispose();
            }

            NavigationManager.LocationChanged -= OnLocationChange;
            StateContainer.OnChange -= StateHasChanged;
        }
    }
}