using ReactiveUI;


namespace Shiny
{
    public interface IValidationViewModel : IReactiveObject // needs to be idisposable or something to destroy
    {
        IValidationBinding Validation { get; }
    }
}
