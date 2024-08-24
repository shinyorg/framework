using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Shiny.Impl;

public class CommandFactory : ICommandFactory
{
    public ICommand Create(Func<Task> onExecute)
    {
        throw new NotImplementedException();
    }

    public ICommand CreateFromTask(Func<Task> onExecute, IObservable<bool>? canExecute = null)
    {
        throw new NotImplementedException();
    }

    public ICommand Create(Action action, IObservable<bool>? canExecute = null)
    {
        throw new NotImplementedException();
    }

    public ICommand CreateFromTask<T>(Func<T, Task> onExecute, IObservable<bool>? canExecute = null)
    {
        throw new NotImplementedException();
    }

    public ICommand Create<T>(Action<T> action, IObservable<bool>? canExecute = null)
    {
        throw new NotImplementedException();
    }
}