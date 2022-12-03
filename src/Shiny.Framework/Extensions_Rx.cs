using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using ReactiveUI;

namespace Shiny;


public static class RxExtensions
{
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
