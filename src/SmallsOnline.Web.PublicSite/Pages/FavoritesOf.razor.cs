using System.Net.Http.Json;

using Microsoft.AspNetCore.Components;

using SmallsOnline.Web.Lib.Models.FavoritesOf.Albums;
using SmallsOnline.Web.Lib.Models.FavoritesOf.Tracks;

namespace SmallsOnline.Web.PublicSite;

public partial class FavoritesOf : ComponentBase
{
    [Inject]
    private IHttpClientFactory HttpClientFactory { get; set; } = null!;

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

    private async Task GetFavorites()
    {
        using HttpClient httpClient = HttpClientFactory.CreateClient("PublicApi");
        _albumItems = await httpClient.GetFromJsonAsync<List<AlbumData>?>($"api/favoriteAlbums/{ListYear}");
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