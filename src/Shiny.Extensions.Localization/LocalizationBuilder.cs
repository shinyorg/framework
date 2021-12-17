using Shiny.Extensions.Localization.Impl;
using System;
using System.Collections.Generic;
using System.Reflection;


namespace Shiny.Extensions.Localization
{
    public class LocalizationBuilder
    {
        readonly Dictionary<string, ILocalizationSource> sources = new();


        // TODO: ie. a source could potential yield multiple resx files
        public LocalizationBuilder AddSource(ILocalizationSource source)
        {
            if (this.sources.ContainsKey(source.Name))
                throw new ArgumentException("There is already a source defined called " + source.Name);

            this.sources.Add(source.Name, source);
            return this;
        }


        public LocalizationBuilder AddResource(string baseName, Assembly assembly)
            => this.AddSource(new ResxLocalizationSource(baseName, assembly));


        public ILocalizationManager Build()
        {
            foreach (var source in this.sources)
                source.Value.Load();

            return new LocalizationManager(this.sources);
        }
    }
}
