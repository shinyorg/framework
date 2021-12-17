using Shiny.Extensions.Localization;
using System.Reflection;

namespace Shiny
{
    public static class ResxLocalizeExtensions
    {
        public static LocalizationBuilder AddResource(this LocalizationBuilder builder, string baseName, Assembly assembly)
        {
            return builder;
        }
    }
}
