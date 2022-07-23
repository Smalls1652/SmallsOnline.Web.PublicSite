using Microsoft.AspNetCore.Components.Routing;

namespace SmallsOnline.Web.PublicSite.Client.Shared;

public partial class MainLayout : LayoutComponentBase, IDisposable
{
    [Inject]
    protected NavigationManager NavigationManager { get; set; } = null!;

    private ShouldFadeIn _shouldFadeSlideIn = new();
    private bool _isEnableFadeSlideALocationChangeEventMethod;

    protected override void OnInitialized()
    {
        NavigationManager.LocationChanged += EnableFadeSlideInOnPageChange;
        _isEnableFadeSlideALocationChangeEventMethod = true;

        base.OnInitialized();
    }

    private void EnableFadeSlideInOnPageChange(object? sender, LocationChangedEventArgs eventArgs)
    {
        _shouldFadeSlideIn.Enabled = true;
        StateHasChanged();
        NavigationManager.LocationChanged -= EnableFadeSlideInOnPageChange;
        _isEnableFadeSlideALocationChangeEventMethod = false;
    }

    public void Dispose()
    {
        if (_isEnableFadeSlideALocationChangeEventMethod)
        {
            NavigationManager.LocationChanged -= EnableFadeSlideInOnPageChange;
        }
    }
}