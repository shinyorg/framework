using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XF.Material.Forms.UI.Dialogs;


namespace Shiny.Extensions.Dialogs.XfMaterial
{
    public class XfMaterialDialogs : IDialogs
    {
        readonly IPlatform platform;
        public XfMaterialDialogs(IPlatform platform)
            => this.platform = platform;


        public virtual Task Alert(string message, string title = "Confirm")
            => this.platform.InvokeOnMainThreadAsync(() => MaterialDialog.Instance.AlertAsync(message, title));


        public virtual async Task<bool> Confirm(string message, string title = "Confirm", string okText = "OK", string cancelText = "Cancel")
        {
            var result = await this.platform.InvokeOnMainThreadAsync(async () => await MaterialDialog.Instance.ConfirmAsync(message, title, okText, cancelText));
            return result ?? false;
        }


        public virtual Task<string?> Input(string message, string? title = null)
            => this.platform.InvokeOnMainThreadAsync(() => MaterialDialog.Instance.InputAsync(title, message));


        public virtual async Task<string?> ActionSheet(string title, bool allowCancel, params string[] actions)
        {
            var task = allowCancel
                ? await this.platform.InvokeOnMainThreadAsync(() => MaterialDialog.Instance.SelectChoiceAsync(title, actions.ToList()))
                : await this.platform.InvokeOnMainThreadAsync(() => MaterialDialog.Instance.SelectActionAsync(title, actions.ToList()));

            if (task >= 0)
                actions.ElementAt(task);

            return null;
        }


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