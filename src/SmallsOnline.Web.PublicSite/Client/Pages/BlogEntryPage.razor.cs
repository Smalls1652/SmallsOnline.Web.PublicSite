using System.Net.Http.Json;
using Microsoft.JSInterop;
using SmallsOnline.Web.Lib.Models.Blog;

namespace SmallsOnline.Web.PublicSite.Client;

public partial class BlogEntryPage : ComponentBase, IDisposable
{
    [Inject] protected IHttpClientFactory HttpClientFactory { get; set; } = null!;

    [Inject] protected NavigationManager NavigationManager { get; set; } = null!;

    [Inject] protected IJSRuntime JSRuntime { get; set; } = null!;

    [Inject] protected PersistentComponentState AppState { get; set; } = null!;

    [Inject] protected ILogger<BlogEntryPage> PageLogger { get; set; } = null!;

    [Parameter] public string Id { get; set; } = null!;

    [CascadingParameter(Name = "ShouldFadeSlideIn")]
    protected ShouldFadeIn? ShouldFadeSlideIn { get; set; }

    private bool _isFinishedLoading = false;
    private PersistingComponentStateSubscription? _persistenceSubscription;
    private BlogEntry? _blogEntry;
    private string? _blogExcerpt;

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected override async Task OnParametersSetAsync()
    {
        _persistenceSubscription = AppState.RegisterOnPersisting(PersistBlogEntryData);

        await GetBlogEntry();

        if (_blogEntry is not null && _blogEntry.Content is not null)
        {
            _blogExcerpt = _blogEntry.GetExcerpt(
                asPlainText: true
            );
        }

        _isFinishedLoading = true;

        await base.OnParametersSetAsync();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await JSRuntime.InvokeVoidAsync("hljs.highlightAll");

        await base.OnAfterRenderAsync(firstRender);
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
        bool isDataAvailable = AppState.TryTakeFromJson(
            key: "blogEntryData",
            instance: out BlogEntry? restoredData
        );

        if (!isDataAvailable)
        {
            using HttpClient httpClient = HttpClientFactory.CreateClient("PublicApi");
            _blogEntry = await httpClient.GetFromJsonAsync<BlogEntry>($"api/blog/entry/{Id}");
        }
        else
        {
            PageLogger.LogInformation(
                "Blog entry data was persisted from a prerendered state. Restoring that data instead.");
            _blogEntry = restoredData;
        }
    }

    private Task PersistBlogEntryData()
    {
        AppState.PersistAsJson(
            key: "blogEntryData",
            instance: _blogEntry
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

            _blogEntry = null;
        }
    }
}