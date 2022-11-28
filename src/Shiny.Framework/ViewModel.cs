using System.Reactive.Subjects;
using System.Windows.Input;
using ReactiveUI;

namespace Shiny;


public abstract class ViewModel : BaseViewModel,
                                  IInitializeAsync,
                                  INavigatedAware,
                                  IConfirmNavigationAsync
{
    protected ViewModel(BaseServices services) : base(services)
    {
    }


    public virtual Task InitializeAsync(INavigationParameters parameters) => Task.CompletedTask;
    public virtual Task<bool> CanNavigateAsync(INavigationParameters parameters)
        => Task.FromResult(true);

    public virtual void OnNavigatedFrom(INavigationParameters parameters)
        => this.Deactivate();

    public virtual void OnNavigatedTo(INavigationParameters parameters) { }
}
