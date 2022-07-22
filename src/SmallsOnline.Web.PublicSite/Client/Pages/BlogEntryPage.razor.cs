using System.Net.Http.Json;
using SmallsOnline.Web.Lib.Models.Blog;

namespace SmallsOnline.Web.PublicSite.Client;

public partial class BlogEntryPage : ComponentBase
{
    [Inject]
    protected IHttpClientFactory HttpClientFactory { get; set; } = null!;

    [Inject]
    protected NavigationManager NavigationManager { get; set; } = null!;

    [Parameter]
    public string Id { get; set; } = null!;

    private bool _isFinishedLoading = false;
    private BlogEntry? _blogEntry;

    protected override async Task OnParametersSetAsync()
    {
        await GetBlogEntry();
        _isFinishedLoading = true;

        await base.OnParametersSetAsync();
    }

    private void HandleGoBackButtonClick()
    {
        NavigationManager.NavigateTo(
            uri: "/blog/list/1",
            forceLoad: false
        );
    }

    private async Task GetBlogEntry()
    {
        using HttpClient httpClient = HttpClientFactory.CreateClient("PublicApi");

        _blogEntry = await httpClient.GetFromJsonAsync<BlogEntry>($"api/blog/entry/{Id}");
    }
}