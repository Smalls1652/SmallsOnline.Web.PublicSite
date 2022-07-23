using Microsoft.AspNetCore.Components.Routing;

namespace SmallsOnline.Web.PublicSite.Client.Shared;

public partial class MainLayout : LayoutComponentBase, IDisposable
{
    [Inject]
    protected NavigationManager NavigationManager { get; set; } = null!;

    private ShouldFadeIn _shouldFadeSlideIn = new();
    private bool _isEnableFadeSlideALocationChangeEventMethod;

    protected override void OnAfterRender(bool firstRender)
    {
        if (firstRender)
        {
            NavigationManager.LocationChanged += EnableFadeSlideInOnPageChange;
            _isEnableFadeSlideALocationChangeEventMethod = true;
        }

        base.OnAfterRender(firstRender);
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