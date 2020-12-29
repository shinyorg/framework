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


        public string GetString(string key, CultureInfo culture = null) => this.resources.GetString(key, culture);
        public string this[string key] => this.resources.GetString(key);
    }
}
