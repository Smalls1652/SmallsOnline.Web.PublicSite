using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.JSInterop;

namespace SmallsOnline.Web.PublicSite.Client.Shared;

public partial class MainLayout : LayoutComponentBase, IDisposable
{
    [Inject]
    protected NavigationManager NavigationManager { get; set; } = null!;

    [Inject]
    protected IJSRuntime JSRuntime { get; set; } = null!;

    private IJSObjectReference? _mainLayoutJSModule;
    private ShouldFadeIn _shouldFadeSlideIn = new();
    private bool _isEnableFadeSlideALocationChangeEventMethod;
    private readonly Regex _anchorTagRegex = new("^(?>https|http):\\/\\/.+?\\/.*(?'anchorTag'#(?'anchorTagName'.+))$");

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _mainLayoutJSModule = await JSRuntime.InvokeAsync<IJSObjectReference>("import", "./Shared/MainLayout.razor.js");

            NavigationManager.LocationChanged += EnableFadeSlideInOnPageChange;

            _isEnableFadeSlideALocationChangeEventMethod = true;
        }

        await ScrollToAnchorAsync(NavigationManager.Uri);

        base.OnAfterRender(firstRender);
    }

    protected override void OnInitialized()
    {
        //NavigationManager.LocationChanged += ScrollToAnchorOnLocationChange;

        base.OnInitialized();
    }

    private void EnableFadeSlideInOnPageChange(object? sender, LocationChangedEventArgs eventArgs)
    {
        _shouldFadeSlideIn.Enabled = true;
        StateHasChanged();
        NavigationManager.LocationChanged -= EnableFadeSlideInOnPageChange;
        _isEnableFadeSlideALocationChangeEventMethod = false;
    }

    private async void ScrollToAnchorOnLocationChange(object? sender, LocationChangedEventArgs eventArgs)
    {
        await ScrollToAnchorAsync(eventArgs.Location);
    }

    private async Task ScrollToAnchorAsync(string? inputLocation)
    {
        string location;
        if (inputLocation is null)
        {
            location = NavigationManager.Uri;
        }
        else
        {
            location = inputLocation;
        }

        Match anchorTagMatch = _anchorTagRegex.Match(location);

        if (anchorTagMatch.Success == true && anchorTagMatch.Groups["anchorTagName"].Value is not null)
        {
            await _mainLayoutJSModule!.InvokeVoidAsync("scrollToAnchorId", anchorTagMatch.Groups["anchorTagName"].Value);
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            if (_isEnableFadeSlideALocationChangeEventMethod)
            {
                NavigationManager.LocationChanged -= EnableFadeSlideInOnPageChange;
            }
        }
    }
}