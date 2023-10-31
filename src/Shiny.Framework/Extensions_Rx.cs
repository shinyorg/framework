using System;
using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using ReactiveUI;

namespace Shiny;


public static class RxExtensions
{
    public static void AddRange(this CompositeDisposable disposer, IEnumerable<IDisposable> en)
    {
        if (en != null)
            foreach (var dispose in en)
                disposer.Add(dispose);
    }


    public static IDisposable SubOnMainThread<T>(this IObservable<T> obs, Action<T> onNext)
        => obs
            .ObserveOn(RxApp.MainThreadScheduler)
            .Subscribe(onNext);


    public static IDisposable SubOnMainThread<T>(this IObservable<T> obs, Action<T> onNext, Action<Exception> onError)
        => obs
            .ObserveOn(RxApp.MainThreadScheduler)
            .Subscribe(onNext, onError);


    public static IDisposable SubOnMainThread<T>(this IObservable<T> obs, Action<T> onNext, Action<Exception> onError, Action onComplete)
        => obs
            .ObserveOn(RxApp.MainThreadScheduler)
            .Subscribe(onNext, onError, onComplete);
}
