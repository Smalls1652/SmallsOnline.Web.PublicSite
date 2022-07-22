using Microsoft.AspNetCore.Components.Routing;

namespace SmallsOnline.Web.PublicSite.Client.Shared;

public partial class MainLayout : LayoutComponentBase
{
    [Inject]
    protected NavigationManager NavigationManager { get; set; } = null!;

    private ShouldFadeIn _shouldFadeSlideIn = new();

    protected override void OnInitialized()
    {
        NavigationManager.LocationChanged += EnableFadeSlideInOnPageChange;

        base.OnInitialized();
    }

    private void EnableFadeSlideInOnPageChange(object? sender, LocationChangedEventArgs eventArgs)
    {
        _shouldFadeSlideIn.Enabled = true;
        StateHasChanged();
        NavigationManager.LocationChanged -= EnableFadeSlideInOnPageChange;
    }
}