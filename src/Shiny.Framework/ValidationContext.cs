using System;
using System.Collections.Generic;


namespace Shiny
{
    public class ValidationContext
    {
        public IDictionary<string, string> Errors { get; set; } // TODO: this needs to be observable
        public IDictionary<string, bool> Touched { get; set; } // TODO: this needs to be observable
    }
}
