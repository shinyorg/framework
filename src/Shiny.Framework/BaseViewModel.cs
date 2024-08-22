using System.Threading.Tasks;
using Prism.AppModel;
using Prism.Navigation;

namespace Shiny;


public abstract class BaseViewModel : ViewModel,
                                      IInitializeAsync,
                                      IPageLifecycleAware,
                                      IApplicationLifecycleAware,
                                      INavigatedAware,
                                      IConfirmNavigationAsync
{
    protected BaseViewModel(BaseServices services) : base(services) { }

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
