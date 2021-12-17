using System;
using System.Threading.Tasks;


namespace Shiny
{
    public interface IDialogs
    {
        Task Alert(string message, string title = "Error");
        Task<string?> ActionSheet(string title, bool allowCancel, params string[] options);
        Task<bool> Confirm(string message, string title = "Confirm", string okText = "OK", string cancelText = "Cancel");
        Task<string?> Input(string question, string? title = null);
        Task<bool> Snackbar(string message, int durationMillis = 3000, string? actionText = null);
        Task<IAsyncDisposable> LoadingSnackbar(string message);
        Task<IAsyncDisposable> LoadingDialog(string message);
    }
}
