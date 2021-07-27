using System.Reactive.Threading.Tasks;
using System.Threading;
using System.Threading.Tasks;


namespace Shiny.UserDialogs
{
    public static class AsyncExtensions
    {
        public static Task AlertAsync(this IUserDialogs dialogs, AlertOptions options, CancellationToken cancelToken = default)
            => dialogs.Alert(options).ToTask(cancelToken);
    }
}
