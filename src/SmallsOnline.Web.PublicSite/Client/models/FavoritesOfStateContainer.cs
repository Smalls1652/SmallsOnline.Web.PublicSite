namespace SmallsOnline.Web.PublicSite.Client.Models;

public class FavoritesOfStateContainer
{
    private string? _savedListYear;

    public string? ListYear
    {
        get => _savedListYear;
        set
        {
            _savedListYear = value;
            NotifyStateChanged();
        }
    }

    public event Action? OnChange;

    private void NotifyStateChanged() => OnChange?.Invoke();
}