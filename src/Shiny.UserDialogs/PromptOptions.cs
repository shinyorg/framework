using System;

namespace Shiny.UserDialogs
{
    public class PromptOptions : DialogOptions
    {
        public string? Value { get; set; }
        public string? Placeholder { get; set; }
        public string? PositiveText { get; set; }
        public string? NeutralText { get; set; }
        public string? NegativeText { get; set; }
        public PromptEntryType? EntryType { get; set; }
        public Func<string, bool>? CanSubmit { get; set; }
    }


    public enum PromptEntryType
    {
        DecimalNumber,
        Email,
        Name,
        Number,
        NumericPassword,
        Password,
        Phone,
        Url
    }
}
