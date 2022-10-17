using System.Reactive.Subjects;
using ReactiveUI;

namespace Shiny;


public abstract class ViewModel : BaseViewModel,
                                  IInitializeAsync,
                                  INavigatedAware,
                                  IPageLifecycleAware,
                                  IConfirmNavigationAsync
{
    protected ViewModel(BaseServices services) : base(services)
    {
    }


    public virtual Task InitializeAsync(INavigationParameters parameters) => Task.CompletedTask;
    public virtual void OnAppearing() {}
    public virtual void OnDisappearing() => this.Deactivate();


    public virtual void OnNavigatedFrom(INavigationParameters parameters)
        => this.navSubj?.OnNext((parameters, false));


    public virtual void OnNavigatedTo(INavigationParameters parameters)
        => this.navSubj?.OnNext((parameters, true));


    public virtual Task<bool> CanNavigateAsync(INavigationParameters parameters)
        => Task.FromResult(true);


    Subject<(INavigationParameters, bool)>? navSubj;
    public IObservable<(INavigationParameters Paramters, bool NavigatedTo)> WhenNavigation()
    {
        navSubj ??= new Subject<(INavigationParameters, bool)>();
        return navSubj.DisposedBy(this.DestroyWith);
    }
}
