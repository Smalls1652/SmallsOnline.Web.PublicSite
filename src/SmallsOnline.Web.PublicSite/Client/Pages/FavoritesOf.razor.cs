using System.Net.Http.Json;

using SmallsOnline.Web.Lib.Models.FavoritesOf.Albums;
using SmallsOnline.Web.Lib.Models.FavoritesOf.Songs;

namespace SmallsOnline.Web.PublicSite.Client;

/// <summary>
/// The page for displaying the favorite music of a given year.
/// </summary>
public partial class FavoritesOf : ComponentBase, IDisposable
{
    [Inject]
    protected IHttpClientFactory HttpClientFactory { get; set; } = null!;

    [Inject]
    protected PersistentComponentState AppState { get; set; } = null!;

    [Inject]
    protected ILogger<BlogEntryPage> PageLogger { get; set; } = null!;

    [Parameter]
    public string? ListYear { get; set; }

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
        _isFinishedLoading = false;

        _persistenceSubscription = AppState.RegisterOnPersisting(PersistFavoritesOfData);

        await GetFavorites();

        _isFinishedLoading = true;
    }

    /// <summary>
    /// Get the favorite music items from the API.
    /// </summary>
    private async Task GetFavorites()
    {
        bool isAlbumItemsDataAvailable = AppState.TryTakeFromJson(
            key: "albumItemsData",
            instance: out List<AlbumData>? restoredAlbumItemsData
        );

        bool isTrackItemsDataAvailable = AppState.TryTakeFromJson(
            key: "trackItemsData",
            instance: out List<SongData>? restoredTrackItemsData
        );

        if (!isAlbumItemsDataAvailable)
        {
            using HttpClient httpClient = HttpClientFactory.CreateClient("PublicApi");
            
            // Get the favorite albums from the API.
            _albumItems = await httpClient.GetFromJsonAsync<List<AlbumData>?>($"api/favoriteAlbums/{ListYear}");
        }
        else
        {
            PageLogger.LogInformation("Album list data was persisted from a prerendered state. Restoring that data instead.");
            _albumItems = restoredAlbumItemsData;
        }

        if (!isTrackItemsDataAvailable)
        {
            using HttpClient httpClient = HttpClientFactory.CreateClient("PublicApi");

            // Get the favorite tracks from the API.
            _trackItems = await httpClient.GetFromJsonAsync<List<SongData>?>($"api/favoriteTracks/{ListYear}");
        }
        else
        {
            PageLogger.LogInformation("Track list data was persisted from a prerendered state. Restoring that data instead.");
            _trackItems = restoredTrackItemsData;
        }
    }

    private Task PersistFavoritesOfData()
    {
        AppState.PersistAsJson(
            key: "albumItemsData",
            instance: _albumItems
        );

        AppState.PersistAsJson(
            key: "trackItemsData",
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

            _albumItems = null;
            _trackItems = null;
        }
    }
}