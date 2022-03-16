using Microsoft.AspNetCore.Components;

namespace SmallsOnline.Web.PublicSite.Shared.Navigation;

public partial class NavbarCollapseSection : ComponentBase
{
    [CascadingParameter(Name = "Collapsed")]
    protected bool Collapsed { get; set; }

    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    [Parameter]
    public Action? ToggleChildCollapse { get; set; }
}