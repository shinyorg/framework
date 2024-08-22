using System.Reactive.Linq;
using System.Windows.Input;

namespace Shiny.Applications;


public class ShinyCommand : ICommand
{
    public event EventHandler? CanExecuteChanged;

    public bool CanExecute(object? parameter)
    {
        throw new NotImplementedException();
    }

    public void Execute(object? parameter)
    {
        throw new NotImplementedException();
    }
}


public class ShinyCommand<T> : ICommand, IDisposable
{
    Action<T> onExecute;
    IDisposable disposer;


    public ShinyCommand(Action<T> onExecute, IObservable<bool> whenCanExecute)
    {
        this.onExecute = onExecute;
        this.disposer = whenCanExecute
            .Where(x => this.canExecute != x)
            .Subscribe(x =>
            {
                this.canExecute = x;
                this.CanExecuteChanged?.Invoke(this, EventArgs.Empty);
            });
    }


    bool canExecute = true;

    public event EventHandler? CanExecuteChanged;
    public bool CanExecute(object? parameter) => this.canExecute;


    public void Execute(object? parameter)
    {
        //this.onExecute(parameter);
    }

    public void Dispose()
    {
        this.disposer.Dispose();
    }
}