using System;
using System.Threading.Tasks;


namespace Shiny.Extensions.Dialogs
{
    public class AsyncDisposable : IAsyncDisposable
    {
        public static IAsyncDisposable Create(Func<ValueTask> dispose) => new AsyncDisposable(dispose);

        readonly Func<ValueTask> dispose;
        public AsyncDisposable(Func<ValueTask> dispose) => this.dispose = dispose;
        public ValueTask DisposeAsync() => this.dispose.Invoke();
    }
}