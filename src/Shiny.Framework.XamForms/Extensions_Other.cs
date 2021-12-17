using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using ReactiveUI;


namespace Shiny
{
    public static partial class Extensions
    {
        public static async Task<AccessState> OpenAppSettingsIf(this IDialogs dialogs, Func<Task<AccessState>> accessRequest, string deniedMessage, string restrictedMessage)
        {
            var result = await accessRequest.Invoke();

            switch (result)
            {
                case AccessState.Denied:
                    await dialogs.SnackbarToOpenAppSettings(deniedMessage);
                    break;

                case AccessState.Restricted:
                    await dialogs.SnackbarToOpenAppSettings(restrictedMessage);
                    break;
            }

            return result;
        }


        public static Task PickEnumValue<T>(this IDialogs dialogs, string title, Action<T> action, Func<T, string>? translate = null) where T : Enum
        {
            var keys = Enum.GetNames(typeof(T));
            var actions = new List<(string Key, Action Action)>(keys.Length);
            if (translate == null)
            {
                var localize = ShinyHost.Resolve<ILocalize>();
                if (localize == null)
                    translate = key => key.ToString();
                else
                    translate = key => localize.GetEnum<T>(key);
            }

            foreach (var key in keys)
            {
                var value = (T)Enum.Parse(typeof(T), key);
                var text = translate(value);
                actions.Add((text, () => action(value)));
            }
            return dialogs.ActionSheet(title, false, actions.ToArray());
        }


        public static ICommand PickEnumValueCommand<T>(this IDialogs dialogs, string title, Action<T> action, Func<T, string>? translate = null, IObservable<bool>? canExecute = null) where T : Enum =>
            ReactiveCommand.CreateFromTask(() => dialogs.PickEnumValue(title, action, translate), canExecute);


        public static string GetEnum<T>(this ILocalize localize, T value)
        {
            if (!typeof(T).IsEnum)
                throw new ArgumentException(typeof(T).FullName + " is not an enum");

            var name = $"{typeof(T).Namespace}.{typeof(T).Name}.{value}";
            return localize[name];
        }


        public static string GetEnum<T>(this ILocalize localize, string value)
        {
            if (!typeof(T).IsEnum)
                throw new ArgumentException(typeof(T).FullName + " is not an enum");

            var name = $"{typeof(T).Namespace}.{typeof(T).Name}.{value}";
            return localize[name];
        }


        public static ICommand ConfirmCommand(this IDialogs dialogs, Func<Task> action, string question, string title = "Confirm", string okText = "OK", string cancelText = "Cancel") => ReactiveCommand.CreateFromTask(async _ =>
        {
            var result = await dialogs.Confirm(question, title, okText, cancelText);
            if (result)
                await action().ConfigureAwait(false);
        });
    }
}
