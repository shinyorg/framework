using System;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using Prism;
using Prism.AppModel;
using Prism.Navigation;
using ReactiveUI.Fody.Helpers;


namespace Shiny
{
    public abstract class LifecycleViewModel : BaseViewModel,
                                               IActiveAware,
                                               IInitializeAsync,
                                               INavigatedAware,
                                               IPageLifecycleAware,
                                               IConfirmNavigationAsync
    {
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


        /// <summary>
        /// This is not fired and only an artifact from Prism
        /// </summary>
        public event EventHandler? IsActiveChanged;
        [Reactive] public bool IsActive { get; set; }
    }
}
