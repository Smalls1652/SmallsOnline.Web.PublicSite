using Microsoft.AspNetCore.Components;

namespace SmallsOnline.Web.PublicSite.Client;

/// <summary>
/// The index/home page.
/// </summary>
public partial class Home : ComponentBase
{
    [Inject] protected ILogger<Home> PageLogger { get; set; } = null!;

    [CascadingParameter(Name = "ShouldFadeSlideIn")]
    protected ShouldFadeIn? ShouldFadeSlideIn { get; set; }
}