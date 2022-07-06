namespace Shiny;


public interface IValidationViewModel : ReactiveUI.IReactiveObject // needs to be idisposable or something to destroy
{
    IValidationBinding? Validation { get; }
}
