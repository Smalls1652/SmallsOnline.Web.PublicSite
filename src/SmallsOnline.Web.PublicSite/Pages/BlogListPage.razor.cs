using System.Net.Http.Json;
using SmallsOnline.Web.Lib.Models.Blog;

namespace SmallsOnline.Web.PublicSite;

public partial class BlogListPage : ComponentBase
{
    [Inject]
    protected IHttpClientFactory HttpClientFactory { get; set; } = null!;

    [Inject]
    protected NavigationManager NavigationManager { get; set; } = null!;

    [Parameter]
    public int PageNumber { get; set; } = 1;

    private bool _isFinishedLoading = false;
    private BlogEntries? _blogEntries;

    private int _previousPageNumber = 1;
    private bool _previousPageBtnDisabled = false;
    private int _nextPageNumber = 1;
    private bool _nextPageBtnDisabled = false;

    protected override async Task OnParametersSetAsync()
    {
        Uri pageUri = new(NavigationManager.Uri);
        if (pageUri.AbsolutePath == "/blog" || pageUri.AbsolutePath == "/blog/" || pageUri.AbsolutePath == "/blog/list" || pageUri.AbsolutePath == "/blog/list/")
        {
            NavigationManager.NavigateTo(
                uri: "/blog/list/1",
                forceLoad: false
            );
        }

        await GetBlogEntries();

        if (PageNumber == 1)
        {
            _previousPageNumber = 1;
            _previousPageBtnDisabled = true;
        }
        else
        {
            _previousPageNumber = PageNumber - 1;
            _previousPageBtnDisabled = false;
        }

        _nextPageNumber = PageNumber + 1;
        if (PageNumber >= _blogEntries?.TotalPages)
        {
            _nextPageBtnDisabled = true;
        }
        else
        {
            _nextPageBtnDisabled = false;
        }

    _isFinishedLoading = true;

        await base.OnParametersSetAsync();
    }

    private async Task GetBlogEntries()
    {
        using HttpClient httpClient = HttpClientFactory.CreateClient("PublicApi");

        _blogEntries = await httpClient.GetFromJsonAsync<BlogEntries>($"api/blog/entries/{PageNumber}");
    }
}