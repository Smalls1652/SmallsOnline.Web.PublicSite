using System.Net.Http.Json;
using Microsoft.AspNetCore.Components;

using SmallsOnline.Web.Lib.Models.Projects;

namespace SmallsOnline.Web.PublicSite.Shared.Projects;

public partial class ProjectCard : ComponentBase
{
    [Inject]
    private IHttpClientFactory _httpClientFactory { get; set; } = null!;

    [Parameter, EditorRequired]
    public string Name { get; set; } = null!;

    [Parameter, EditorRequired]
    public string Type { get; set; } = null!;

    [Parameter]
    public string? Description { get; set; }

    [Parameter, EditorRequired]
    public Uri Url { get; set; } = null!;

    [Parameter]
    public bool UrlIsRepo { get; set; }

    private string? _buttonText;
    private ProjectType? _projectType;

    private bool _finishedLoading = false;

    protected override async Task OnInitializedAsync()
    {
        _finishedLoading = false;

        SetButtonText();
        await EvaluateProjectType();

        _finishedLoading = true;
    }

    private void SetButtonText()
    {
        if (UrlIsRepo == true)
        {
            _buttonText = "Visit repo";
        }
        else
        {
            _buttonText = "Visit site";
        }
    }

    private async Task EvaluateProjectType()
    {
        List<ProjectType>? projectTypes;

        using (HttpClient httpClient = _httpClientFactory.CreateClient("BaseAppClient"))
        {
            projectTypes = await httpClient.GetFromJsonAsync<List<ProjectType>?>("json/projects/project-types.json");
        }

        if (projectTypes is null)
        {
            throw new(
                message: "Could not retrieve project types data."
            );
        }

        ProjectType? foundProjectType = projectTypes.Find(
            (ProjectType item) => item.Type == Type
        );

        if (foundProjectType is null)
        {
            throw new(
                message: $"Could not match '{Type}' from the known project types."
            );
        }

        _projectType = foundProjectType;
    }
}