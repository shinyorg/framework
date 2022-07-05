namespace Shiny;


public interface IValidationBinding : ReactiveUI.IReactiveObject, IDisposable
{
    IReadOnlyDictionary<string, string> Errors { get; }
    IReadOnlyDictionary<string, bool> Touched { get;}
}
