using System.Globalization;

namespace Shiny.Extensions.Localization
{
    public interface ILocalizationManager
    {
        string? this[string key] { get; }
        string? GetString(string key, CultureInfo? culture = null);
        ILocalizationSource? GetSection(string sectionName);
    }
}
