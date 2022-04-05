namespace SmallsOnline.Web.PublicSite.Shared.Cards;

public partial class InfoCard : ComponentBase
{
    [Parameter]
    public string Title { get; set; } = "⚠️ Note";
    
    [Parameter]
    public RenderFragment? ChildContent { get; set; }
}