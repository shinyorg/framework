using System.Windows.Input;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;


namespace Samples
{
    public class ActionItemViewModel : ReactiveObject
    {
        [Reactive] public string? Text { get; set; }
        [Reactive] public string? Detail { get; set; }
        public ICommand? Command { get; set; }
    }
}
