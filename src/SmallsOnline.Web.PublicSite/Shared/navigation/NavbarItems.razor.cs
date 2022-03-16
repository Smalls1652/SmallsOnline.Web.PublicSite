using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;

using Microsoft.Extensions.Logging;

using Microsoft.JSInterop;

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
    private string? activeItem;

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

        await SetActiveNavItemAsync(currentPage);
    }

    private string? GetCurrentPageFromUri(string uri)
    {
        string? currentPage = null;

        Regex uriSectionRegex = new("^(?:https|http)://(?'hostName'.+?)/(?'topLevelPage'.*?)(?:/.*$|$)", RegexOptions.Multiline);

        Match uriSectionMatch = uriSectionRegex.Match(uri);

        if (uriSectionMatch.Success == false)
        {
            Logger.LogError("Failed to parse the current page to update the navigation bar. Uri provided: {uri}", uri);
        }
        else
        {
            currentPage = uriSectionMatch.Groups["topLevelPage"].Value;
        }

        // If the current page is null, we need to set it as home. 
        if (string.IsNullOrEmpty(currentPage) == true)
        {
            currentPage = "home";
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
        ToggleChildCollapse?.Invoke();
    }

    public void Dispose()
    {
        NavManager.LocationChanged -= OnLocationChangeAsync;
    }
}