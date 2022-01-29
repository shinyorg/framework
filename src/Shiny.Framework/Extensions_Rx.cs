using ReactiveUI;
using System;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reactive.Disposables;
using System.Reactive.Linq;


namespace Shiny
{
    public static class RxExtensions
    {
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
