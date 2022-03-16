using System.Net.Http.Json;

using Microsoft.AspNetCore.Components;

using SmallsOnline.Web.Lib.Models.Projects;

namespace SmallsOnline.Web.PublicSite;

public partial class Projects : ComponentBase
{
    [Inject]
    private IHttpClientFactory HttpClientFactory { get; set; } = null!;

    private List<ProjectItem>? _projectItems;
    private bool _isFinishedLoading = false;

    protected override async Task OnInitializedAsync()
    {
        _isFinishedLoading = false;

        using (HttpClient httpClient = HttpClientFactory.CreateClient("BaseAppClient"))
        {
            _projectItems = await httpClient.GetFromJsonAsync<List<ProjectItem>?>("json/projects/projects-data.json");
        }
        
        _isFinishedLoading = true;
    }
}