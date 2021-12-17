using System;
using System.Threading.Tasks;


namespace Shiny.Extensions.Dialogs
{
    public class EmptyDialogs : IDialogs
    {
        public Task<string?> ActionSheet(string title, string? acceptText = null, string? dismissText = null, params string[] options) => Task.FromResult<string?>(null);
        public Task Alert(string message, string? title = null, string? dismissText = null) => Task.CompletedTask;
        public Task<bool> Confirm(string message, string? title = null, string? acceptText = null, string? dismissText = null) => Task.FromResult(false);
        public Task<string?> Input(string question, string? title = null, string? acceptText = null, string? dismissText = null, string? placeholder = null, int? maxLength = null) => Task.FromResult<string?>(null);
        public Task<IAsyncDisposable> LoadingDialog(string message) => Task.FromResult(AsyncDisposable.Create(async () => { }));
        public Task<IAsyncDisposable> LoadingSnackbar(string message) => Task.FromResult(AsyncDisposable.Create(async () => { }));
        public Task<bool> Snackbar(string message, int durationMillis = 3000, string? actionText = null) => Task.FromResult(false);
    }
}
