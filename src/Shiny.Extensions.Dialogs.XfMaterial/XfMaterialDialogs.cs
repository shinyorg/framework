using System;
using System.Linq;
using System.Threading.Tasks;
using XF.Material.Forms.UI.Dialogs;


namespace Shiny.Extensions.Dialogs.XfMaterial
{
    public class XfMaterialDialogs : IDialogs
    {
        readonly IPlatform platform;
        public XfMaterialDialogs(IPlatform platform) => this.platform = platform;


        public virtual async Task<string?> ActionSheet(string title, string? acceptText = null, string? dismissText = null, params string[] actions)
        {
            var task = await this.platform.InvokeOnMainThreadAsync(() => MaterialDialog.Instance.SelectChoiceAsync(
                title,
                actions.ToList(),
                acceptText,
                dismissText,
                null,
                true
            ));
            if (task >= 0)
                return actions.ElementAt(task);

            return null;
        }


        public virtual Task Alert(string message, string? title = null, string? dismissText = null)
            => this.platform.InvokeOnMainThreadAsync(() => MaterialDialog.Instance.AlertAsync(message, title, dismissText ?? "OK"));


        public virtual async Task<bool> Confirm(string message, string? title = null, string? acceptText = null, string? dismissText = null)
        {
            var result = await this.platform.InvokeOnMainThreadAsync(async () => await MaterialDialog.Instance.ConfirmAsync(
                message,
                title,
                acceptText,
                dismissText
            ));
            return result ?? false;
        }


        public Task<string?> Input(string question, string? title = null, string? acceptText = null, string? dismissText = null, string? placeholder = null, int? maxLength = null)
            => this.platform.InvokeOnMainThreadAsync(() => MaterialDialog.Instance.InputAsync(
                title,
                question,
                null,
                placeholder,
                acceptText,
                dismissText
            ));


        public Task<bool> Snackbar(string message, int durationMillis = 3000, string? actionText = null)
            => this.platform.InvokeOnMainThreadAsync(async () =>
            {
                if (actionText == null)
                {
                    await MaterialDialog.Instance.SnackbarAsync(message, durationMillis);
                    return false;
                }
                return await MaterialDialog.Instance.SnackbarAsync(message, actionText, durationMillis);
            });


        public async Task<IAsyncDisposable> LoadingSnackbar(string message)
        {
            var dialog = await this.platform.InvokeOnMainThreadAsync(() => MaterialDialog.Instance.LoadingSnackbarAsync(message));
            return AsyncDisposable.Create(async () => await this.platform.InvokeOnMainThreadAsync(() => dialog.DismissAsync()));
        }


        public async Task<IAsyncDisposable> LoadingDialog(string message)
        {
            var dialog = await this.platform.InvokeOnMainThreadAsync(() => MaterialDialog.Instance.LoadingDialogAsync(message));
            return AsyncDisposable.Create(async () => await this.platform.InvokeOnMainThreadAsync(() => dialog.DismissAsync()));
        }
    }
}