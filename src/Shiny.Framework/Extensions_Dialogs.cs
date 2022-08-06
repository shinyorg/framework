using Shiny.Hosting;

namespace Shiny;


public static class DialogExtensions
{
    //public static async Task<T> LoadingTask<T>(this IDialogs dialogs, Func<Task<T>> task, string message, bool useSnackbar = false)
    //{
    //    var showDialog = useSnackbar
    //        ? dialogs.LoadingSnackbar(message)
    //        : dialogs.LoadingDialog(message);

    //    var dialogEl = await showDialog.ConfigureAwait(false);

    //    await using (dialogEl.ConfigureAwait(false))
    //        return await task.Invoke().ConfigureAwait(false);
    //}


    //public static async Task LoadingTask(this IDialogs dialogs, Func<Task> task, string message = "Loading", bool useSnackbar = false)
    //{
    //    var showDialog = useSnackbar
    //        ? dialogs.LoadingSnackbar(message)
    //        : dialogs.LoadingDialog(message);

    //    var dialogEl = await showDialog.ConfigureAwait(false);

    //    await using (dialogEl.ConfigureAwait(false))
    //        await task.Invoke().ConfigureAwait(false);
    //}


    public static async Task<AccessState> OpenAppSettingsIf(this IDialogs dialogs, Func<Task<AccessState>> accessRequest, string deniedMessage, string restrictedMessage)
    {
        var result = await accessRequest.Invoke();

        switch (result)
        {
            case AccessState.Denied:
                await dialogs.SnackbarToOpenAppSettings(deniedMessage);
                break;

            case AccessState.Restricted:
                await dialogs.SnackbarToOpenAppSettings(restrictedMessage);
                break;
        }

        return result;
    }


    public static async Task SnackbarToOpenAppSettings(this IDialogs dialogs, string message, string actionText = "Open", int durationMillis = 3000)
    {
        var appInfo = Host.Current.Services.GetRequiredService<IAppInfo>();

        var result = await dialogs.Snackbar(
            message,
            durationMillis,
            actionText
        );
        if (result)
            appInfo.ShowSettingsUI();
    }
}
