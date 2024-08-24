using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Shiny;


public interface IShinyCommand : ICommand, INotifyPropertyChanged
{
    public bool IsRunning { get; }
    Task ExecuteAsync(CancellationToken cancellationToken = default);
}

public interface IShinyCommand<T> : ICommand, INotifyPropertyChanged
{
    bool IsRunning { get; }
    Task ExecuteAsync(T parameter, CancellationToken cancellationToken = default);
}