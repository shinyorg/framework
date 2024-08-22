namespace Shiny.Applications;


public interface IDialogs
{
    // TODO: tri-state, action sheet, loading, progress
    Task Alert( string message, string? title = null, string buttonText = "OK");

    Task Confirm(
        string message,
        string? title = null,
        string confirmText = "Yes",
        string cancelText = "No"
    );
}