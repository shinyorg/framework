using System.Reactive.Threading.Tasks;
using System.Threading;
using System.Threading.Tasks;


namespace Shiny.UserDialogs
{
    public static class Extensions
    {
        public static Task<string?> ActionSheetAsync(this IUserDialogs dialogs, ActionSheetOptions options, CancellationToken cancelToken = default)
            => dialogs.ActionSheet(options).ToTask(cancelToken);

        public static Task AlertAsync(this IUserDialogs dialogs, AlertOptions options, CancellationToken cancelToken = default)
            => dialogs.Alert(options).ToTask(cancelToken);

        public static Task<bool> ConfirmAsync(this IUserDialogs dialogs, ConfirmOptions options, CancellationToken cancelToken = default)
            => dialogs.Confirm(options).ToTask(cancelToken);

        public static Task<PromptResult> PromptAsync(this IUserDialogs dialogs, PromptOptions options, CancellationToken cancelToken = default)
            => dialogs.Prompt(options).ToTask(cancelToken);
    }
}
