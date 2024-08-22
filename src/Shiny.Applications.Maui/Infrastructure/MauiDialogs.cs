namespace Shiny.Applications.Maui.Infrastructure;


public class MauiDialogs : IDialogs
{
    public MauiDialogs()
    {
        //Shell.Current.CurrentPage
    }


    public Task Alert(string message, string? title = null, string buttonText = "OK")
    {
        throw new NotImplementedException();
    }

    public Task Confirm(string message, string? title = null, string confirmText = "Yes", string cancelText = "No")
    {
        throw new NotImplementedException();
    }
}