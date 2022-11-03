using System.Net.Http.Json;
using SmallsOnline.Web.Lib.Models.Blog;

namespace SmallsOnline.Web.PublicSite.Client;

public partial class BlogListPage : ComponentBase, IDisposable
{
    [Inject] protected IHttpClientFactory HttpClientFactory { get; set; } = null!;

    [Inject] protected NavigationManager NavigationManager { get; set; } = null!;

    [Inject] protected PersistentComponentState AppState { get; set; } = null!;

    [Inject] protected ILogger<BlogEntryPage> PageLogger { get; set; } = null!;

    [Parameter] public int PageNumber { get; set; } = 1;

    [CascadingParameter(Name = "ShouldFadeSlideIn")]
    protected ShouldFadeIn? ShouldFadeSlideIn { get; set; }

    private bool _isFinishedLoading = false;
    private PersistingComponentStateSubscription? _persistenceSubscription;
    private BlogEntries? _blogEntries;

    private int _previousPageNumber = 1;
    private bool _previousPageBtnDisabled = false;
    private int _nextPageNumber = 1;
    private bool _nextPageBtnDisabled = false;

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected override async Task OnParametersSetAsync()
    {
        _persistenceSubscription = AppState.RegisterOnPersisting(PersistBlogListData);

        Uri pageUri = new(NavigationManager.Uri);
        if (pageUri.AbsolutePath == "/blog" || pageUri.AbsolutePath == "/blog/" ||
            pageUri.AbsolutePath == "/blog/list" || pageUri.AbsolutePath == "/blog/list/")
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
        bool isDataAvailable = AppState.TryTakeFromJson(
            key: "blogListData",
            instance: out BlogEntries? restoredData
        );

        if (!isDataAvailable)
        {
            using HttpClient httpClient = HttpClientFactory.CreateClient("PublicApi");

            _blogEntries = await httpClient.GetFromJsonAsync<BlogEntries>($"api/blog/entries/{PageNumber}");
        }
        else
        {
            PageLogger.LogInformation(
                "Blog list data was persisted from a prerendered state. Restoring that data instead.");
            _blogEntries = restoredData;
        }
    }

    private Task PersistBlogListData()
    {
        AppState.PersistAsJson(
            key: "blogListData",
            instance: _blogEntries
        );

        return Task.CompletedTask;
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            if (_persistenceSubscription.HasValue)
            {
                _persistenceSubscription.Value.Dispose();
            }

            _blogEntries = null;
        }
    }
}