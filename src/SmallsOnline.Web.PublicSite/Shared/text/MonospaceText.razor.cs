namespace SmallsOnline.Web.PublicSite.Shared.Text;

public partial class MonospaceText : ComponentBase
{
    [Parameter]
    public RenderFragment? ChildContent { get; set; }
}