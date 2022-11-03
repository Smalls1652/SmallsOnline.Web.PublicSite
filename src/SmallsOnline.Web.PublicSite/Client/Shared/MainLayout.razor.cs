using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.JSInterop;

namespace SmallsOnline.Web.PublicSite.Client.Shared;

public partial class MainLayout : LayoutComponentBase, IDisposable
{
    [Inject] protected NavigationManager NavigationManager { get; set; } = null!;

    [Inject] protected ILogger<MainLayout> Logger { get; set; } = null!;

    [Inject] protected IJSRuntime JSRuntime { get; set; } = null!;

    private IJSObjectReference? _mainLayoutJSModule;
    private ShouldFadeIn _shouldFadeSlideIn = new();
    private bool _isEnableFadeSlideInOnLocationChangeEventMethod;
    private readonly Regex _anchorTagRegex = new("^(?>https|http):\\/\\/.+?\\/.*(?'anchorTag'#(?'anchorTagName'.+))$");

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _mainLayoutJSModule =
                await JSRuntime.InvokeAsync<IJSObjectReference>("import", "./Shared/MainLayout.razor.js");

            NavigationManager.LocationChanged += EnableFadeSlideInOnPageChange;
            _isEnableFadeSlideInOnLocationChangeEventMethod = true;

            await ScrollToAnchorAsync(NavigationManager.Uri);
        }

        base.OnAfterRender(firstRender);
    }

    protected override void OnInitialized()
    {
        NavigationManager.LocationChanged += ScrollToAnchorOnLocationChange;

        base.OnInitialized();
    }

    private void EnableFadeSlideInOnPageChange(object? sender, LocationChangedEventArgs eventArgs)
    {
        _shouldFadeSlideIn.Enabled = true;
        StateHasChanged();
        NavigationManager.LocationChanged -= EnableFadeSlideInOnPageChange;
        _isEnableFadeSlideInOnLocationChangeEventMethod = false;
    }

    /// <summary>
    /// Handles scrolling to an element on location change events.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="eventArgs"></param>
    private async void ScrollToAnchorOnLocationChange(object? sender, LocationChangedEventArgs eventArgs)
    {
        await ScrollToAnchorAsync(eventArgs.Location);
    }

    /// <summary>
    /// Initiate a scroll to an element.
    /// </summary>
    /// <param name="inputLocation">The current Uri.</param>
    private async Task ScrollToAnchorAsync(string? inputLocation)
    {
        // Initialize the location based on the input parameter.
        string location;
        if (inputLocation is null)
        {
            // If 'inputLocation' was null, then set the location to the current
            // Uri reported by 'NavigationManager'.
            location = NavigationManager.Uri;
        }
        else
        {
            // Otherwise, set the location the provided input.
            location = inputLocation;
        }

        // Run a regex match on the location.
        Match anchorTagMatch = _anchorTagRegex.Match(location);

        // If the regex match was successful and the 'anchorTagName' group's value is not null,
        // then attempt to scroll to the element.
        if (anchorTagMatch.Success == true && anchorTagMatch.Groups["anchorTagName"].Value is not null)
        {
            /*
                Loop for a max of 4 times while attempting to scroll.
                We don't want to have this go on infinitely, so we're going to cap it at 4 times.
                The reason for doing this is because of a limitation of where we're handling the logic.
                If the logic was handled on the pages, we would have to define the logic on each page.
                Doing it on the main layout that handles the routing between pages, allows us to define it
                once; however, the main layout has no idea if the page has finished loading. This causes the
                initial attempt to potentially fail because the specified element doesn't exist at that point in time.
            */

            bool scrollWasSuccessful = false;
            for (int i = 1; i < 5 && scrollWasSuccessful == false; i++)
            {
                try
                {
                    // Attempt to scroll to the element specified in the anchor using JavaScript interop.
                    await _mainLayoutJSModule!.InvokeVoidAsync("scrollToAnchorId",
                        anchorTagMatch.Groups["anchorTagName"].Value);

                    // If no exception was thrown, then set 'scrollWasSuccessful' to true to end the loop early.
                    scrollWasSuccessful = true;
                }
                catch (JSException e)
                {
                    // If an exception was thrown while executing the JS, then attemp to log a warning message
                    // and wait 1000ms (1s) to try again.
                    Logger.LogWarning("'{ErrorMessage}' was thrown during loop {LoopCount}.", e.Message, i);
                    await Task.Delay(1000);
                }
            }
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
            if (_isEnableFadeSlideInOnLocationChangeEventMethod)
            {
                NavigationManager.LocationChanged -= EnableFadeSlideInOnPageChange;
            }

            NavigationManager.LocationChanged -= ScrollToAnchorOnLocationChange;
        }
    }
}