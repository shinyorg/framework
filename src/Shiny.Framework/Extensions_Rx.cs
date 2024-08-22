using System;
using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Threading;
using System.Threading.Tasks;

namespace Shiny;


public static class RxExtensions
{
    /// <summary>
    /// Allows you to await an observable, while still consuming events
    /// The await will return when the observable completes or errors
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="observable"></param>
    /// <param name="onNext"></param>
    /// <param name="cancelToken"></param>
    /// <returns></returns>
    public static async Task ToAsyncWithNotifications<T>(this IObservable<T> observable, Action<T> onNext, CancellationToken cancelToken = default)
    {
        IDisposable? sub = null;
        try
        {
            var tcs = new TaskCompletionSource();
            using var _ = cancelToken.Register(() => tcs.TrySetCanceled());

            sub = observable.Subscribe(
                onNext,
                ex => tcs.TrySetException(ex),
                () => tcs.TrySetResult()
            );

            await tcs.Task.ConfigureAwait(false);
        }
        finally
        {
            sub?.Dispose();
        }
    }


    /// <summary>
    /// Adds multiple items to a composite disposable
    /// </summary>
    /// <param name="disposer"></param>
    /// <param name="en"></param>
    public static void AddRange(this CompositeDisposable disposer, IEnumerable<IDisposable> en)
    {
        if (en != null)
            foreach (var dispose in en)
                disposer.Add(dispose);
    }


    public static IObservable<T> ObserveOnMainThread<T>(this IObservable<T> obs)
        => obs; //.ObserveOn(RxApp.MainThreadScheduler);


    public static IDisposable SubOnMainThread<T>(this IObservable<T> obs, Action<T> onNext)
        => obs
            .ObserveOnMainThread()
            .Subscribe(onNext);


    public static IDisposable SubOnMainThread<T>(this IObservable<T> obs, Action<T> onNext, Action<Exception> onError)
        => obs
            .ObserveOnMainThread()
            .Subscribe(onNext, onError);


    public static IDisposable SubOnMainThread<T>(this IObservable<T> obs, Action<T> onNext, Action<Exception> onError, Action onComplete)
        => obs
            .ObserveOnMainThread()
            .Subscribe(onNext, onError, onComplete);
}
