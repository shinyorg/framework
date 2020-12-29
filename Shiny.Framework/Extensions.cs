using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Prism.Navigation;
using ReactiveUI;


namespace Shiny
{
    public static class Extensions
    {
        public static async Task<AccessState> OpenAppSettingsIf(this IDialogs dialogs, Func<Task<AccessState>> accessRequest, string deniedMessageKey, string restrictedMessageKey)
        {
            var result = await accessRequest.Invoke();
            var localize = ShinyHost.Resolve<ILocalize>();

            switch (result)
            {
                case AccessState.Denied:
                    var deniedMsg = localize?.GetString(deniedMessageKey) ?? deniedMessageKey;
                    await dialogs.SnackbarToOpenAppSettings(deniedMsg);
                    break;

                case AccessState.Restricted:
                    var restrictMsg = localize?.GetString(restrictedMessageKey) ?? restrictedMessageKey;
                    await dialogs.SnackbarToOpenAppSettings(restrictMsg);
                    break;
            }

            return result;
        }


        public static Task PickEnumValue<T>(this IDialogs dialogs, string title, Action<T> action)
        {
            var keys = Enum.GetNames(typeof(T));
            var actions = new List<(string Key, Action Action)>(keys.Length);

            foreach (var key in keys)
            {
                var value = (T)Enum.Parse(typeof(T), key);
                actions.Add((key, () => action(value)));
            }
            return dialogs.ActionSheet(title, false, actions.ToArray());
        }


        public static ICommand PickEnumValueCommand<T>(this IDialogs dialogs, string title, Action<T> action, IObservable<bool>? canExecute = null) =>
            ReactiveCommand.CreateFromTask(() => dialogs.PickEnumValue(title, action), canExecute);


        public static async Task<bool> RequestAccess(this IDialogs dialogs, Func<Task<AccessState>> request)
        {
            var access = await request();
            return await dialogs.AlertAccess(access);
        }


        public static async Task<bool> AlertAccess(this IDialogs dialogs, AccessState access)
        {
            switch (access)
            {
                case AccessState.Available:
                    return true;

                case AccessState.Restricted:
                    await dialogs.Alert("WARNING: Access is restricted");
                    return true;

                default:
                    await dialogs.Alert("Invalid Access State: " + access);
                    return false;
            }
        }


        public static string GetEnum<T>(this ILocalize localize, T value)
        {
            if (!typeof(T).IsEnum)
                throw new ArgumentException(typeof(T).FullName + " is not an enum");

            var name = $"{typeof(T).Namespace}.{typeof(T).Name}.{value}";
            return localize[name];
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

        public static Task Navigate(this INavigationService navigation, string uri, params (string, object)[] parameters)
            => navigation.Navigate(uri, parameters.ToNavParams());


        public static async Task Navigate(this INavigationService navigation, string uri, INavigationParameters parameters)
            => (await navigation.NavigateAsync(uri, parameters)).Assert();


        public static void Assert(this INavigationResult result)
        {
            if (!result.Success)
            {
                Console.WriteLine("[NAV FAIL] " + result.Exception);
                throw new ArgumentException("Failed to navigate", result.Exception);
            }
        }

        public static ICommand NavigateCommand(this INavigationService navigation, string uri, Action<INavigationParameters> getParams = null, IObservable<bool> canExecute = null)
            => ReactiveCommand.CreateFromTask(async () =>
            {
                var p = new NavigationParameters();
                getParams?.Invoke(p);
                await navigation.Navigate(uri, p);
            }, canExecute);


        public static ICommand NavigateCommand<T>(this INavigationService navigation, string uri, Action<T, INavigationParameters> getParams = null, IObservable<bool> canExecute = null)
            => ReactiveCommand.CreateFromTask<T>(async arg =>
            {
                var p = new NavigationParameters();
                getParams?.Invoke(arg, p);
                await navigation.Navigate(uri, p);
            }, canExecute);



        public static Task GoBack(this INavigationService navigation, bool toRoot = false, params (string, object)[] parameters)
            => navigation.GoBack(toRoot, parameters.ToNavParams());


        public static async Task GoBack(this INavigationService navigation, bool toRoot = false, INavigationParameters parameters = null)
        {
            parameters = parameters ?? new NavigationParameters();
            var task = toRoot
                ? navigation.GoBackToRootAsync(parameters)
                : navigation.GoBackAsync(parameters);

            var result = await task.ConfigureAwait(false);
            result.Assert();
        }


        public static ICommand GoBackCommand(this INavigationService navigation, bool toRoot = false, Action<INavigationParameters> getParams = null, IObservable<bool> canExecute = null)
            => ReactiveCommand.CreateFromTask(async () =>
            {
                var p = new NavigationParameters();
                getParams?.Invoke(p);
                await navigation.GoBack(toRoot, p);
            }, canExecute);


        public static ICommand GoBackCommand<T>(this INavigationService navigation, bool toRoot = false, Action<T, INavigationParameters> getParams = null, IObservable<bool> canExecute = null)
            => ReactiveCommand.CreateFromTask<T>(async arg =>
            {
                var p = new NavigationParameters();
                getParams?.Invoke(arg, p);
                await navigation.GoBack(toRoot, p);
            }, canExecute);


        public static INavigationParameters Set(this INavigationParameters parameters, string key, object value)
        {
            parameters.Add(key, value);
            return parameters;
        }


        public static INavigationParameters AddRange(this INavigationParameters parameters, params (string Key, object Value)[] args)
        {
            foreach (var arg in args)
                parameters.Add(arg.Key, arg.Value);

            return parameters;
        }


        static NavigationParameters ToNavParams(this (string Key, object Value)[] parameters)
        {

            var navParams = new NavigationParameters();
            if (parameters != null && parameters.Any())
                navParams.AddRange(parameters);

            return navParams;
        }
    }
}
