using System;
using System.Globalization;
using System.Reflection;
using System.Resources;


namespace Shiny.Impl
{
    public class ResxLocalize : ILocalize
    {
        readonly ResourceManager resources;


        public ResxLocalize(string baseName, Assembly assembly)
            => this.resources = new ResourceManager(baseName, assembly);


        public string GetString(string key, CultureInfo? culture = null)
        {
            var value = this.resources.GetString(key, culture);
            if (value == null)
                return "KEY '{key}' not found";

            return value;
        }


        public string this[string key] => this.GetString(key);
    }
}
