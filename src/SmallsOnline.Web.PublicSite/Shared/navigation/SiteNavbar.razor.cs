using Microsoft.AspNetCore.Components;

using Microsoft.Extensions.Logging;

namespace SmallsOnline.Web.PublicSite.Shared.Navigation;

public partial class SiteNavbar : ComponentBase
{
    [Inject]
    private ILogger<SiteNavbar>? Logger { get; set; }

    [CascadingParameter]
    public bool Collapsed { get; set; } = true;

    private void ToggleCollapse()
    {
        Collapsed = !Collapsed;
        StateHasChanged();

        Logger.LogInformation("Collapsed toggled. New value: {Collapsed}", Collapsed);
    }

    private void ChildComponentRequestedCollapse()
    {
        Logger.LogInformation("Child component has requested that the navbar collapse be toggled.");
        ToggleCollapse();
        StateHasChanged();
    }
}