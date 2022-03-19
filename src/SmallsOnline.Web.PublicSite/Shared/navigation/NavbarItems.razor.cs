using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;

using Microsoft.Extensions.Logging;

using Microsoft.JSInterop;

using SmallsOnline.Web.PublicSite.Models;

namespace SmallsOnline.Web.PublicSite.Shared.Navigation;

public partial class NavbarItems : ComponentBase, IDisposable
{
    [Inject]
    private NavigationManager NavManager { get; set; } = null!;

    [Inject]
    private ILogger<NavbarItems> Logger { get; set; } = null!;

    [Inject]
    private IJSRuntime JSRuntime { get; set; } = null!;

    [CascadingParameter(Name = "ToggleChildCollapse")]
    public Action? ToggleChildCollapse { get; set; }

    private IJSObjectReference? navbarItemsJsModule;

    private CurrentPageLocation? _currentLocationInfo;
    private string? activeItem;
    private bool _topMusicDropDownOpened = false;

    protected override async Task OnInitializedAsync()
    {
        navbarItemsJsModule = await JSRuntime.InvokeAsync<IJSObjectReference>("import", "./Shared/navigation/NavbarItems.razor.js");
        string? currentPage = GetCurrentPageFromUri(NavManager.Uri);
        Logger.LogInformation("Starting page: {currentPage}", currentPage);
        NavManager.LocationChanged += OnLocationChangeAsync;

        await SetActiveNavItemAsync(currentPage);
    }

    private async void OnLocationChangeAsync(object? sender, LocationChangedEventArgs eventArgs)
    {
        string? currentPage = GetCurrentPageFromUri(eventArgs.Location);
        Logger.LogInformation("Page changed: {currentPage}", currentPage);
        Logger.LogInformation("Current path: {Path}", _currentLocationInfo!.Path);

        await SetActiveNavItemAsync(currentPage);
    }

    private string? GetCurrentPageFromUri(string uri)
    {
        string? currentPage = null;

        try
        {
            _currentLocationInfo = new(uri);
            currentPage = _currentLocationInfo.TopLevelPage;

            // If the current page is null, we need to set it as home. 
            if (string.IsNullOrEmpty(currentPage) == true)
            {
                currentPage = "home";
            }
        }
        catch (Exception e)
        {
            Logger.LogError("{Message}", e.Message);
        }

        return currentPage;
    }

    private async Task SetActiveNavItemAsync(string? currentPage)
{
    if (navbarItemsJsModule != null)
    {
        if (string.IsNullOrEmpty(activeItem) == false)
        {
            await navbarItemsJsModule.InvokeVoidAsync("removeActiveClass", $"navitem_{activeItem}");
        }

        activeItem = currentPage;
        await navbarItemsJsModule.InvokeVoidAsync("setActiveClass", $"navitem_{activeItem}");

        StateHasChanged();
    }
}

private void ToggleNavbarCollapsed()
{
    _topMusicDropDownOpened = false;
    ToggleChildCollapse?.Invoke();
}

private void ToggleTopMusicDropdown()
{
    _topMusicDropDownOpened = !_topMusicDropDownOpened;
}

public void Dispose()
{
    NavManager.LocationChanged -= OnLocationChangeAsync;
}
}