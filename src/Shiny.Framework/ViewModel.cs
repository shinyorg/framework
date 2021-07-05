using System;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using Prism.AppModel;
using Prism.Navigation;


namespace Shiny
{
    public abstract class ViewModel : BaseViewModel,
                                      IInitializeAsync,
                                      INavigatedAware,
                                      IPageLifecycleAware,
                                      IConfirmNavigationAsync
    {
        public virtual Task InitializeAsync(INavigationParameters parameters) => Task.CompletedTask;
        public virtual void OnAppearing() {}
        public virtual void OnDisappearing() => this.Deactivate();


        public virtual void OnNavigatedFrom(INavigationParameters parameters)
        {
            this.navFromSubj?.OnNext(parameters);
        }


        public virtual void OnNavigatedTo(INavigationParameters parameters)
        {
            this.navToSubj?.OnNext(parameters);
        }


        public virtual Task<bool> CanNavigateAsync(INavigationParameters parameters) => Task.FromResult(true);


        Subject<INavigationParameters>? navToSubj;
        public IObservable<INavigationParameters> WhenNavigatedTo()
        {
            navToSubj ??= new Subject<INavigationParameters>();
            return navToSubj.DisposedBy(this.DestroyWith);
        }


        Subject<INavigationParameters>? navFromSubj;
        public IObservable<INavigationParameters> WhenNavigatedFrom()
        {
            navFromSubj ??= new Subject<INavigationParameters>();
            return navFromSubj.DisposedBy(this.DestroyWith);
        }
    }
}
