namespace SmallsOnline.Web.PublicSite.Client.Shared.Navigation;

/// <summary>
/// The navigation bar for displaying branding and links to pages.
/// </summary>
public partial class SiteNavbar : ComponentBase
{
    [Inject] protected ILogger<SiteNavbar> Logger { get; set; } = null!;

    [CascadingParameter] public bool Collapsed { get; set; } = true;

    /// <summary>
    /// Toggle the collapsed section of the navigation bar.
    /// </summary>
    private void ToggleCollapse()
    {
        // Flip the current value of 'Collapsed'.
        Collapsed = !Collapsed;

        // Initiate a state change.
        StateHasChanged();

        Logger.LogInformation("Collapsed toggled. New value: {Collapsed}", Collapsed);
    }

    /// <summary>
    /// An action that child components invoke to cause the collapse section to change.
    /// </summary>
    private void ChildComponentRequestedCollapse()
    {
        Logger.LogInformation("Child component has requested that the navbar collapse be toggled.");

        // Trigger a toggle of the navigation bar.
        ToggleCollapse();
    }
}