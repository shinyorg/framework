using System.Reactive.Subjects;
using System.Threading.Tasks;
using System.Windows.Input;
using ReactiveUI;

namespace Shiny;


public abstract class ViewModel : BaseViewModel,
                                  IInitializeAsync,
                                  IPageLifecycleAware,
                                  IApplicationLifecycleAware,
                                  INavigatedAware,
                                  IConfirmNavigationAsync
{
    protected ViewModel(BaseServices services) : base(services) {}

    public virtual Task InitializeAsync(INavigationParameters parameters)
        => Task.CompletedTask;

    public virtual Task<bool> CanNavigateAsync(INavigationParameters parameters)
        => Task.FromResult(true);

    public virtual void OnNavigatedFrom(INavigationParameters parameters)
        => this.Deactivate();

    public virtual void OnNavigatedTo(INavigationParameters parameters) { }
    public virtual void OnAppearing() { }
    public virtual void OnDisappearing() { }

    public virtual void OnResume() => this.OnAppearing();
    public virtual void OnSleep() => this.OnDisappearing();
}
