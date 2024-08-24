using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Shiny;


public interface ICommandFactory
{
    ICommand CreateFromTask(Func<Task> onExecute, IObservable<bool>? canExecute = null);
    ICommand Create(Action action, IObservable<bool>? canExecute = null);

    ICommand CreateFromTask<T>(Func<T, Task> onExecute, IObservable<bool>? canExecute = null);
    ICommand Create<T>(Action<T> action, IObservable<bool>? canExecute = null);
}