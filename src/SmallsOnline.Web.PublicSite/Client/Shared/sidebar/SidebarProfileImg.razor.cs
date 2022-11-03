using Microsoft.AspNetCore.Components;

namespace SmallsOnline.Web.PublicSite.Client.Shared.Sidebar;

/// <summary>
/// Component for handling the sidebar "profile pic" image.
/// </summary>
public partial class SidebarProfileImg : ComponentBase
{
    /// <summary>
    /// Logger for the component.
    /// </summary>
    [Inject]
    protected ILogger<SidebarProfileImg> ComponentLogger { get; set; } = null!;

    /// <summary>
    /// What the current animation class is set to.
    /// </summary>
    private string? _currentAnimationClass = "";

    /// <summary>
    /// Has the image been set to "Dumb mode"
    /// </summary>
    private bool _isSetToDumb = false;

    /// <summary>
    /// The current count of clicks.
    /// </summary>
    private int _clickCounter = 0;

    /// <summary>
    /// Handles clicks on the image.
    /// </summary>
    private async Task HandleImageClick()
    {
        // Increment _clickCounter by 1.
        _clickCounter++;
        if (_clickCounter < 15)
        {
            // If _clickCounter is less than 15, then only log a message to the console with the amount of current clicks.
            ComponentLogger.LogInformation("Times clicked: {_clickCounter}", _clickCounter);
        }
        else
        {
            // If _clickCounter is 15 or higher, then invoke the image change process.
            ComponentLogger.LogInformation("I can't believe you've done this.");
            await InvokeImageChange();

            // Reset _clickCounter back to 0.
            _clickCounter = 0;
        }
    }

    /// <summary>
    /// Changes the image from the normal image to the dumb image.
    /// Also handles the spin in and spin out animations during the process.
    /// </summary>
    private async Task InvokeImageChange()
    {
        // Set the current animation to 'spin-out'.
        // Then wait 800ms to let the animation finish.
        _currentAnimationClass = "spin-out";
        await Task.Delay(800);

        // Flip the current value of _isSetToDumb.
        _isSetToDumb = !_isSetToDumb;

        // Set the current animation to 'spin-in'.
        _currentAnimationClass = "spin-in";

        // This fixes a bug with the state not changing properly in-between the two animations.
        // If this isn't performed, the 'spin-in' animation will not be applied.
        await Task.Delay(1);
        StateHasChanged();

        // Wait 800ms to let the 'spin-in' animation finish.
        await Task.Delay(800);

        // Remove the animation class value.
        _currentAnimationClass = null;
    }
}