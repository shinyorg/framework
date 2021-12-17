using Shiny.Extensions.Localization;

using System;
using System.Linq;
using System.Reflection;


namespace Shiny
{
    public static class LocalizationExtensions
    {
        public static void Bind(this ILocalizationSource localize, object obj)
        {
            var props = obj
                .GetType()
                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Where(x => x.SetMethod != null && x.GetMethod != null)
                .ToList();

            foreach (var prop in props)
            {
                var value = localize[prop.Name];
                if (value != null)
                    prop.SetValue(obj, value);
            }
        }


        public static string? GetEnum<T>(this ILocalizationManager localize, string section, T value)
            => localize.GetSection(section)?.GetEnum(value);


        public static string? GetEnum<T>(this ILocalizationSource localize, T value)
        {
            if (!typeof(T).IsEnum)
                throw new ArgumentException(typeof(T).FullName + " is not an enum");

            var name = $"{typeof(T).Namespace}.{typeof(T).Name}.{value}";
            return localize[name];
        }
    }
}
