namespace SmallsOnline.Web.PublicSite.Client;

/// <summary>
/// The about me page.
/// </summary>
public partial class AboutMe : ComponentBase
{
    [CascadingParameter(Name = "ShouldFadeSlideIn")]
    protected ShouldFadeIn? ShouldFadeSlideIn { get; set; }
}