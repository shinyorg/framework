using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XF.Material.Forms.UI.Dialogs;


namespace Shiny.Impl
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


        public virtual Task ActionSheet(string title, bool allowCancel, params (string Key, Action Action)[] actions)
        {
            var dict = new Dictionary<string, Action>();
            foreach (var action in actions)
                dict.Add(action.Key, action.Action);

            return this.ActionSheet(title, dict, allowCancel);
        }


        public virtual async Task ActionSheet(string title, IDictionary<string, Action> actions, bool allowCancel = false)
        {
            var task = allowCancel
                ? await this.platform.InvokeOnMainThreadAsync(() => MaterialDialog.Instance.SelectChoiceAsync(title, actions.Keys.ToList()))
                : await this.platform.InvokeOnMainThreadAsync(() => MaterialDialog.Instance.SelectActionAsync(title, actions.Keys.ToList()));

            if (task >= 0)
                actions.Values.ElementAt(task).Invoke();
        }


        public virtual async Task<T> LoadingTask<T>(Func<Task<T>> task, string message, bool useSnackbar = false)
        {
            var result = default(T);
            IMaterialModalPage? dialog = null;
            try
            {
                dialog = useSnackbar
                    ? await this.platform.InvokeOnMainThreadAsync(() => MaterialDialog.Instance.LoadingSnackbarAsync(message))
                    : await this.platform.InvokeOnMainThreadAsync(() => MaterialDialog.Instance.LoadingDialogAsync(message));

                result = await task();
                await this.platform.InvokeOnMainThreadAsync(() => dialog.DismissAsync());
            }
            catch
            {
                if (dialog != null)
                    await this.platform.InvokeOnMainThreadAsync(() => dialog.DismissAsync());
                throw;
            }
            return result;
        }


        public virtual Task LoadingTask(Func<Task> task, string message = "Loading", bool useSnackbar = false)
            => this.LoadingTask<object?>(async () =>
            {
                await task();
                return null;
            },
            message,
            useSnackbar
        );


        public virtual Task Snackbar(string message, int durationMillis = 3000)
            => this.platform.InvokeOnMainThreadAsync(() => MaterialDialog.Instance.SnackbarAsync(message, durationMillis));


        public async Task SnackbarToOpenAppSettings(string message, string actionButtonText = "Open", int durationMillis = 3000)
        {
            var result = await this.platform.InvokeOnMainThreadAsync(() => MaterialDialog.Instance.SnackbarAsync(message, actionButtonText, durationMillis));
            if (result)
                Xamarin.Essentials.AppInfo.ShowSettingsUI();
        }
    }
}