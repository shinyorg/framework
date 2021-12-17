using System;
using System.Threading.Tasks;


namespace Shiny.Extensions.Dialogs
{
    public class EmptyDialogs : IDialogs
    {
        public Task<string?> ActionSheet(string title, bool allowCancel, params string[] options) => Task.FromResult<string?>(null);
        public Task Alert(string message, string title = "Error") => Task.CompletedTask;
        public Task<bool> Confirm(string message, string title = "Confirm", string okText = "OK", string cancelText = "Cancel") => Task.FromResult(true);
        public Task<string?> Input(string question, string? title = null) => Task.FromResult<string?>(null);
        public Task<IAsyncDisposable> LoadingDialog(string message) => Task.FromResult(AsyncDisposable.Create(async () => { }));
        public Task<IAsyncDisposable> LoadingSnackbar(string message) => Task.FromResult(AsyncDisposable.Create(async () => { }));
        public Task<bool> Snackbar(string message, int durationMillis = 3000, string? actionText = null) => Task.FromResult(false);
    }
}
