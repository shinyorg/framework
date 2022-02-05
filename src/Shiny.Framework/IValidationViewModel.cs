using ReactiveUI;
using System.Collections.Generic;


namespace Shiny
{
    public interface IValidationViewModel : IReactiveObject // needs to be idisposable or something to destroy
    {
        IDictionary<string, string> Errors { get; set; } // TODO: this needs to be observable
        IDictionary<string, bool> Touched { get; set; } // TODO: this needs to be observable
    }
}
