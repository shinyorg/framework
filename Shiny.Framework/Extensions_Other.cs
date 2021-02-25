using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using ReactiveUI;


namespace Shiny
{
    //using System;


    //namespace Shiny
    //{
    //    public struct DateRange
    //    {
    //        public DateRange(DateTimeOffset start, DateTimeOffset end)
    //        {
    //            this.Start = start;
    //            this.End = end;
    //        }


    //        public DateTimeOffset Start { get; }
    //        public DateTimeOffset End { get; }
    //    }


    //    public static partial class Extensions
    //    {
    //        public static DateRange GetRangeForDate(this DateTimeOffset date) => new DateRange(date.Date, date.GetEndOfDay());


    //        public static DateTime GetEndOfDay(this DateTimeOffset date)
    //            => date.Date.AddDays(1);
    //    }
    //}

    public static partial class Extensions
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
    }
}
