using System.Globalization;


namespace Shiny.Extensions.Localization
{
    public interface ILocalizationSource
    {
        string Name { get; }
        string? this[string key] { get; }
        string? GetString(string key, CultureInfo? culture = null);
        void Load();
    }
}
