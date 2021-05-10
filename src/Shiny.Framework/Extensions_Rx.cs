using System;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ReactiveUI;
using Shiny.Impl;
using Shiny.Infrastructure;


namespace Shiny
{
    public static partial class Extensions
    {
        public static void UseXfMaterialDialogs(this IServiceCollection services)
            => services.TryAddSingleton<IDialogs, XfMaterialDialogs>();

        public static void UseGlobalCommandExceptionHandler(this IServiceCollection services, Action<GlobalExceptionHandlerConfig>? configure = null)
        {
            services.AddSingleton<GlobalExceptionHandler>();
            configure?.Invoke(GlobalExceptionHandlerConfig.Instance);
        }

        public static void UseResxLocalization(this IServiceCollection services, Assembly assembly, string resourceName)
            => services.AddSingleton<ILocalize>(new ResxLocalize(resourceName, assembly));


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


        ///// <summary>
        ///// Will watch for changes in any observable item in the ObservableCollection
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="collection"></param>
        ///// <returns></returns>
        //public static IObservable<ItemChanged<T, string>> WhenItemChanged<T>(this ObservableCollection<T> collection)
        //    where T : INotifyPropertyChanged
        //    => Observable.Create<ItemChanged<T, string>>(ob =>
        //    {
        //        var disp = new CompositeDisposable();
        //        foreach (var item in collection)
        //            disp.Add(item.WhenAnyProperty().Subscribe(ob.OnNext));

        //        return disp;
        //    });


        ///// <summary>
        ///// Will watch for a specific property change with any item in the ObservableCollection
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <typeparam name="TRet"></typeparam>
        ///// <param name="collection"></param>
        ///// <param name="expression"></param>
        ///// <returns></returns>
        //public static IObservable<ItemChanged<T, TRet>> WhenItemValueChanged<T, TRet>(
        //    this ObservableCollection<T> collection,
        //    Expression<Func<T, TRet>> expression) where T : INotifyPropertyChanged =>
        //    Observable.Create<ItemChanged<T, TRet>>(ob =>
        //    {
        //        var disp = new CompositeDisposable();
        //        foreach (var item in collection)
        //        {
        //            disp.Add(item
        //                .WhenAnyProperty(expression)
        //                .Subscribe(x => ob.OnNext(new ItemChanged<T, TRet>(item, x)))
        //            );
        //        }

        //        return disp;
        //    });


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
