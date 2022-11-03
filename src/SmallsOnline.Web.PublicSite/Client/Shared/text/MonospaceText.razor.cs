namespace SmallsOnline.Web.PublicSite.Client.Shared.Text;

public partial class MonospaceText : ComponentBase
{
    [Parameter] public RenderFragment? ChildContent { get; set; }
}