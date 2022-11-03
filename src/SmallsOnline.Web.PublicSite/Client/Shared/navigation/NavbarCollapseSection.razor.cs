namespace SmallsOnline.Web.PublicSite.Client.Shared.Navigation;

/// <summary>
/// A collapse section in the navigation bar.
/// </summary>
public partial class NavbarCollapseSection : ComponentBase
{
    /// <summary>
    /// Whether or not the section is currently collapsed.
    /// </summary>
    [CascadingParameter(Name = "Collapsed")]
    protected bool Collapsed { get; set; }

    [Parameter] public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Action for indicating the collapse section has been toggled.
    /// </summary>
    [Parameter]
    public Action? ToggleChildCollapse { get; set; }
}