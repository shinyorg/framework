namespace Shiny;

public interface IDialogs
{
    Task Alert(string message, string? title = null, string? dismissText = null);
    Task<string?> ActionSheet(string title, string? acceptText = null, string? dismissText = null, params string[] options);
    Task<bool> Confirm(string message, string? title = null, string? acceptText = null, string? dismissText = null);
    Task<string?> Input(string question, string? title = null, string? acceptText = null, string? dismissText = null, string? placeholder = null, int? maxLength = null, InputKeyboard keyboard = InputKeyboard.Default);
    Task<bool> Snackbar(string message, int durationMillis = 3000, string? actionText = null);
}


public enum InputKeyboard
{
    Chat,
    Default,
    Email,
    Numeric,
    Plain,
    Telephone,
    Text,
    Url
}