using Acr.UserDialogs;
using Shiny.Extensions.Localization;
using System;
using System.Threading;
using System.Threading.Tasks;


namespace Shiny.Extensions.Dialogs.AcrUserDialogs
{
    public class AcrUserDialogsImpl : IDialogs
    {
        readonly IUserDialogs userDialogs;
        readonly ILocalizationSource? localizeSource;


        public AcrUserDialogsImpl(IUserDialogs userDialogs, ILocalizationSource? localizeSource)
        {
            this.userDialogs = userDialogs;
            this.localizeSource = localizeSource;
        }


        public Task<string?> ActionSheet(string title, string? acceptText = null, string? dismissText = null, params string[] options)
            => this.userDialogs.ActionSheetAsync(
                title,
                acceptText,
                null,
                CancellationToken.None,
                options
            );


        public Task Alert(string message, string? title = null, string? dismissText = null)
           => this.userDialogs.AlertAsync(message, title, dismissText ?? this.GetText("OK"));

        public Task<bool> Confirm(string message, string? title = null, string? acceptText = null, string? dismissText = null)
            => this.userDialogs.ConfirmAsync(message, title, acceptText ?? this.GetText("OK"), dismissText ?? this.GetText("Cancel"));

        public async Task<string?> Input(string question, string? title = null, string? acceptText = null, string? dismissText = null, string? placeholder = null, int? maxLength = null)
        {
            var result = await this.userDialogs.PromptAsync(question, title, acceptText ?? this.GetText("OK"), dismissText ?? this.GetText("Cancel"));
            return result.Ok ? result.Value : (string?)null;
        }


        public Task<IAsyncDisposable> LoadingDialog(string message)
        {
            var disposable = this.userDialogs.Loading(message);
            return Task.FromResult(AsyncDisposable.Create(() =>
            {
                disposable.Dispose();
                return new ValueTask();
            }));
        }


        public Task<IAsyncDisposable> LoadingSnackbar(string message)
        {
            throw new NotImplementedException("NOT SUPPORTED");
        }


        public Task<bool> Snackbar(string message, int durationMillis = 3000, string? actionText = null)
        {
            var tcs = new TaskCompletionSource<bool>();

            var cfg = new ToastConfig(message)
            {
                Duration = TimeSpan.FromMilliseconds(durationMillis),
            };
            if (actionText != null)
            {
                cfg.SetAction(x =>
                {
                    x.Text = actionText;
                    x.SetAction(() => tcs.TrySetResult(true));
                });
            }

            this.userDialogs.Toast(cfg);
            Task.Delay(durationMillis).ContinueWith(_ => tcs.TrySetResult(false));

            return tcs.Task;
        }


        protected virtual string GetText(string text)
        {
            var localizedText = this.localizeSource?[text];
            return localizedText ?? text;
        }
    }
}