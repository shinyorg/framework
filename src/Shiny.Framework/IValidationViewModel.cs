using ReactiveUI;
using System.Collections.Generic;


namespace Shiny
{
    public interface IValidationViewModel : IReactiveObject // needs to be idisposable or something to destroy
    {
        ValidationContext Validation { get; }
    }
}
