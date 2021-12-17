using System.Linq;
using System.Reflection;


namespace Shiny.Extensions.Localization
{
    public static class BindExtensions
    {
        public static void Bind(this ILocalize localize, object obj)
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
    }
}
