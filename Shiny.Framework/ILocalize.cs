using System;
using System.Globalization;


namespace Shiny
{
    public interface ILocalize
    {
        string this[string key] { get; }
        string GetString(string key, CultureInfo culture);
    }
}
