using System;
using System.Threading.Tasks;


namespace Shiny
{
    public static class DialogExtensions
    {
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


        public static Task SnackbarToOpenAppSettings(this IDialogs dialogs, string message, string actionText = "Open", int durationMillis = 3000)
            => dialogs.Snackbar(
                message,
                durationMillis,
                actionText,
                () => Xamarin.Essentials.AppInfo.ShowSettingsUI()
            );
    }
}
