using System.Text.RegularExpressions;

namespace SmallsOnline.Web.PublicSite.Client.Models;

public class CurrentPageLocation
{
    public CurrentPageLocation(string inputUri)
    {
        Regex uriSectionRegex =
            new("^(?:https|http)://(?'hostName'.+?)(?'path'/(?'topLevelPage'.*?)(?'secondaryPages'/.*?|))(?:#.*|)$",
                RegexOptions.Multiline);

        Match uriSectionMatch = uriSectionRegex.Match(inputUri);

        if (uriSectionMatch.Success == false)
        {
            throw new($"Failed to parse the current page to update the navigation bar. Uri provided: {inputUri}");
        }
        else
        {
            HostName = uriSectionMatch.Groups["hostName"].Value;
            Path = uriSectionMatch.Groups["path"].Value;
            TopLevelPage = uriSectionMatch.Groups["topLevelPage"].Value;
            SecondaryPages = uriSectionMatch.Groups["secondaryPages"].Value;
        }
    }

    public string HostName { get; set; } = null!;

    public string Path { get; set; } = null!;

    public string? TopLevelPage { get; set; }

    public string? SecondaryPages { get; set; }
}