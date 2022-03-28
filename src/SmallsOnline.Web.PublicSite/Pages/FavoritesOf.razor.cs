using System.Net.Http.Json;

using SmallsOnline.Web.Lib.Models.FavoritesOf.Albums;
using SmallsOnline.Web.Lib.Models.FavoritesOf.Tracks;

namespace SmallsOnline.Web.PublicSite;

/// <summary>
/// The page for displaying the favorite music of a given year.
/// </summary>
public partial class FavoritesOf : ComponentBase
{
    [Inject]
    protected IHttpClientFactory HttpClientFactory { get; set; } = null!;

    [Parameter]
    public string? ListYear { get; set; }

    private List<AlbumData>? _albumItems;
    private List<TrackData>? _trackItems;
    private bool _isFinishedLoading = false;
    private ElementReference _favoriteAlbumsRef;
    private ElementReference _favoriteTracksRef;

    protected override async Task OnParametersSetAsync()
    {
        _isFinishedLoading = false;

        await GetFavorites();

        _isFinishedLoading = true;
    }

    /// <summary>
    /// Get the favorite music items from the API.
    /// </summary>
    private async Task GetFavorites()
    {
        using HttpClient httpClient = HttpClientFactory.CreateClient("PublicApi");

        // Get the favorite albums from the API.
        _albumItems = await httpClient.GetFromJsonAsync<List<AlbumData>?>($"api/favoriteAlbums/{ListYear}");

        // Get the favorite tracks from the API.
        _trackItems = await httpClient.GetFromJsonAsync<List<TrackData>?>($"api/favoriteTracks/{ListYear}");
    }

    private async Task ScrollToFavoriteAlbums()
    {
        await _favoriteAlbumsRef.FocusAsync();
    }

    private async Task ScrollToFavoriteTracks()
    {
        await _favoriteTracksRef.FocusAsync();
    }
}