using System;
using System.Threading.Tasks;


namespace Shiny.Dialogs
{
    public static class Extensions
    {
        public static async Task<T> LoadingTask<T>(this IDialogs dialogs, Func<Task<T>> task, string message, bool useSnackbar = false)
        {
            var showDialog = useSnackbar
                ? dialogs.LoadingSnackbar(message)
                : dialogs.LoadingDialog(message);

            var dialogEl = await showDialog.ConfigureAwait(false);

            await using (dialogEl.ConfigureAwait(false))
                return await task.Invoke().ConfigureAwait(false);
        }


        public static async Task LoadingTask(this IDialogs dialogs, Func<Task> task, string message = "Loading", bool useSnackbar = false)
        {
            var showDialog = useSnackbar
                ? dialogs.LoadingSnackbar(message)
                : dialogs.LoadingDialog(message);

            var dialogEl = await showDialog.ConfigureAwait(false);

            await using (dialogEl.ConfigureAwait(false))
                await task.Invoke().ConfigureAwait(false);
        }
    }
}
