using Prism.Services;
using Shiny.Extensions.Dialogs;
using System;
using System.Threading.Tasks;


namespace Shiny.Framework.Impl
{
    public class NativeDialogs : IDialogs
    {
        readonly IPageDialogService prism;
        public NativeDialogs(IPageDialogService prism) => this.prism = prism;


        public async Task<string?> ActionSheet(string title, string? acceptText = null, string? dismissText = null, params string[] options)
        {
            var tcs = new TaskCompletionSource<string>();
            await this.prism.DisplayActionSheetAsync(
                title,
                acceptText,
                dismissText,
                options
            );
            return await tcs.Task.ConfigureAwait(false);
        }

        public Task Alert(string message, string? title = null, string? dismissText = null)
            => this.prism.DisplayAlertAsync(title, message, dismissText ?? "OK");

        public Task<bool> Confirm(string message, string? title = null, string? okText = null, string? cancelText = null)
            => this.prism.DisplayAlertAsync(title, message, okText, cancelText);

        public Task<string?> Input(string question, string? title = null, string? acceptText = null, string? dismissText = null, string? placeholder = null, int? maxLength = null)
            => this.prism.DisplayPromptAsync(title, question, acceptText, dismissText, placeholder, maxLength ?? -1);

        public Task<IAsyncDisposable> LoadingDialog(string message) => Task.FromResult(AsyncDisposable.Create(async () => { }));
        public Task<IAsyncDisposable> LoadingSnackbar(string message) => Task.FromResult(AsyncDisposable.Create(async () => { }));
        public Task<bool> Snackbar(string message, int durationMillis = 3000, string? actionText = null) => Task.FromResult(false);
    }
}
