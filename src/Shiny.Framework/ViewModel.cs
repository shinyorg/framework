using Prism;
using Prism.AppModel;
using Prism.Navigation;
using ReactiveUI.Fody.Helpers;
using System;
using System.Reactive.Subjects;
using System.Threading.Tasks;


namespace Shiny
{
    public abstract class ViewModel : BaseViewModel,
                                      IActiveAware,
                                      IInitializeAsync,
                                      INavigatedAware,
                                      IPageLifecycleAware,
                                      IConfirmNavigationAsync
    {
        public virtual Task InitializeAsync(INavigationParameters parameters) => Task.CompletedTask;
        public virtual void OnAppearing() { }
        public virtual void OnDisappearing() => Deactivate();


        public virtual void OnNavigatedFrom(INavigationParameters parameters)
            => navSubj?.OnNext((parameters, false));


        public virtual void OnNavigatedTo(INavigationParameters parameters)
            => navSubj?.OnNext((parameters, true));


        public virtual Task<bool> CanNavigateAsync(INavigationParameters parameters)
            => Task.FromResult(true);

        private Subject<(INavigationParameters, bool)>? navSubj;
        public IObservable<(INavigationParameters Paramters, bool NavigatedTo)> WhenNavigation()
        {
            navSubj ??= new Subject<(INavigationParameters, bool)>();
            return navSubj.DisposedBy(DestroyWith);
        }


        /// <summary>
        /// This is not fired and only an artifact from Prism
        /// </summary>
        public event EventHandler? IsActiveChanged;
        [Reactive] public bool IsActive { get; set; }
    }
}
