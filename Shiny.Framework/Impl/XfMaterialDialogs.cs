using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XF.Material.Forms.UI.Dialogs;


namespace Shiny.Impl
{
    public class XfMaterialDialogs : IDialogs
    {
        public virtual Task Alert(string message, string title = "Confirm")
            => MaterialDialog.Instance.AlertAsync(message, title);


        public virtual async Task<bool> Confirm(string message, string title = "Confirm", string okText = "OK", string cancelText = "Cancel")
        {
            var result = await MaterialDialog.Instance.ConfirmAsync(message, title, okText, cancelText);
            return result ?? false;
        }


        public virtual Task<string?> Input(string message, string? title = null)
            => MaterialDialog.Instance.InputAsync(title, message);


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
                ? await MaterialDialog.Instance.SelectChoiceAsync(title, actions.Keys.ToList())
                : await MaterialDialog.Instance.SelectActionAsync(title, actions.Keys.ToList());

            if (task >= 0)
                actions.Values.ElementAt(task).Invoke();
        }


        public virtual async Task<T> LoadingTask<T>(Func<Task<T>> task, string message)
        {
            var result = default(T);
            IMaterialModalPage dialog = null;
            try
            {
                dialog = await MaterialDialog.Instance.LoadingDialogAsync(message);
                result = await task();
                await dialog.DismissAsync();
            }
            catch
            {
                await dialog?.DismissAsync();
                throw;
            }
            return result;
        }


        public virtual Task LoadingTask(Func<Task> task, string message = "Loading")
            => this.LoadingTask<object>(async () =>
            {
                await task();
                return null;
            },
            message
        );


        public virtual Task Snackbar(string message)
            => MaterialDialog.Instance.SnackbarAsync(message);
    }
}