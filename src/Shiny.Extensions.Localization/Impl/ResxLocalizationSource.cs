using System;
using System.Globalization;
using System.Reflection;
using System.Resources;


namespace Shiny.Extensions.Localization.Impl
{
    public class ResxLocalizationSource : ILocalizationSource
    {
        readonly ResourceManager resources;

        public ResxLocalizationSource(string baseName, Assembly assembly)
        {
            this.Name = baseName;
            this.resources = new ResourceManager(baseName, assembly);
        }


        public string? GetString(string key, CultureInfo? culture = null)
        {
            var value = this.resources.GetString(key, culture);
            if (value == null)
                return $"KEY '{key}' not found";

            return value;
        }


        public string Name { get; }
        public void Load() {}
        public string? this[string key] => this.GetString(key);
    }
}
