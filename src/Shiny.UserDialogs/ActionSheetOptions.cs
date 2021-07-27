using System;
using System.Collections.Generic;


namespace Shiny.UserDialogs
{
    public class ActionSheetOptions : DialogOptions
    {
        public string? DismissText { get; set; }
        public List<string> Options { get; set; } = new List<string>();
    }
}
