using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using ReactiveUI;
using Shiny.Infrastructure;


namespace Shiny
{
    public static partial class Extensions
    {
        public static IObservable<string> ObserveTimeAgo<T>(this T npc, Expression<Func<T, DateTimeOffset>> expression, Func<TimeSpan, string> transformer, TimeSpan? interval = null) where T : INotifyPropertyChanged
            => npc.WhenAnyProperty(expression).Select(x => x.ObserveTimeAgo(transformer, interval)).Switch();


        public static IObservable<string> ObserveTimeAgo(this DateTimeOffset dt, Func<TimeSpan, string> transformer, TimeSpan? intervalTime = null) => Observable
            .Interval(intervalTime ?? TimeSpan.FromSeconds(10))
            .Select(_ => DateTimeOffset.UtcNow.Subtract(dt))
            .Select(x => transformer(x));


        public static IDisposable ApplyValueRangeConstraint<T>(this T npc, Expression<Func<T, int>> expression, int min, int max) where T : INotifyPropertyChanged
        {
            var property = npc.GetPropertyInfo(expression);

            if (!property.CanWrite)
                throw new ArgumentException($"You can only apply value constraints to public setter properties - {npc.GetType()}.{property.Name}");

            var comp = new CompositeDisposable();
            npc
                .WhenAnyProperty(expression)
                .Where(x => x < min)
                .Subscribe(x => property.SetValue(npc, min))
                .DisposedBy(comp);

            npc
                .WhenAnyProperty(expression)
                .Where(x => x > max)
                .Subscribe(x => property.SetValue(npc, max))
                .DisposedBy(comp);

            return comp;
        }


        /// <summary>
        /// This will buffer observable pings and timestamp them until the predicate check does not pass
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="thisObs"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static IObservable<List<Timestamped<T>>> BufferWhile<T>(this IObservable<T> thisObs, Func<T, bool> predicate)
            => Observable.Create<List<Timestamped<T>>>(ob =>
            {
                var list = new List<Timestamped<T>>();
                return thisObs
                    .Timestamp()
                    .Subscribe(x =>
                    {
                        if (predicate(x.Value))
                        {
                            list.Add(x);
                        }
                        else if (list != null)
                        {
                            ob.OnNext(list);
                            list.Clear();
                        }
                    });
            });


        public static IDisposable ApplyMaxLengthConstraint<T>(this T npc, Expression<Func<T, string>> expression, int maxLength) where T : INotifyPropertyChanged
        {
            var property = npc.GetPropertyInfo(expression);

            if (property.PropertyType != typeof(string))
                throw new ArgumentException($"You can only use maxlength constraints on string based properties - {npc.GetType()}.{property.Name}");

            if (!property.CanWrite)
                throw new ArgumentException($"You can only apply maxlength constraints to public setter properties - {npc.GetType()}.{property.Name}");

            return npc
                .WhenAnyProperty(expression)
                .Where(x => x != null && x.Length > maxLength)
                .Subscribe(x =>
                {
                    var value = x.Substring(0, maxLength);
                    property.SetValue(npc, value);
                });
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
}
