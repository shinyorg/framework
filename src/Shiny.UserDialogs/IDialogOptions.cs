namespace Shiny.UserDialogs
{
    public interface IDialogOptions
    {
        string? Title { get; set; }
        string? Message { get; set; }
    }

    public abstract class DialogOptions : IDialogOptions
    {
        public string? Title { get; set; }
        public string? Message { get; set; }
    }
}
