using System.Net.Http.Json;
using SmallsOnline.Web.Lib.Models.Projects;

namespace SmallsOnline.Web.PublicSite.Client;

/// <summary>
/// The projects page.
/// </summary>
public partial class Projects : ComponentBase
{
    [Inject] protected IHttpClientFactory HttpClientFactory { get; set; } = null!;

    [CascadingParameter(Name = "ShouldFadeSlideIn")]
    protected ShouldFadeIn? ShouldFadeSlideIn { get; set; }

    private List<ProjectItem>? _projectItems;
    private List<ProjectType>? _projectTypes;
    private bool _isFinishedLoading = false;

    protected override async Task OnInitializedAsync()
    {
        _isFinishedLoading = false;

        // Get the projects data and the types of projects.
        using (HttpClient httpClient = HttpClientFactory.CreateClient("BaseAppClient"))
        {
            _projectItems = await httpClient.GetFromJsonAsync<List<ProjectItem>?>("json/projects/projects-data.json");
            _projectTypes = await httpClient.GetFromJsonAsync<List<ProjectType>?>("json/projects/project-types.json");
        }

        _isFinishedLoading = true;
    }
}