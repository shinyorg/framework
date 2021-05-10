using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Extensions.Logging;
using Prism.AppModel;
using Prism.Navigation;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Shiny.Net;
using Shiny.Stores;

namespace Shiny
{
    public abstract class ViewModel : ReactiveObject,
                                      IInitializeAsync,
                                      INavigatedAware,
                                      IDestructible,
                                      IConfirmNavigationAsync
    {
        CompositeDisposable? deactivateWith;
        protected CompositeDisposable DeactivateWith => this.deactivateWith ??= new CompositeDisposable();
        protected CompositeDisposable DestroyWith { get; } = new CompositeDisposable();


        protected virtual void Deactivate()
        {
            this.deactivateWith?.Dispose();
            this.deactivateWith = null;
        }


        public virtual Task InitializeAsync(INavigationParameters parameters) => Task.CompletedTask;
        public virtual void OnNavigatedFrom(INavigationParameters parameters) => this.Deactivate();
        public virtual void OnNavigatedTo(INavigationParameters parameters)
        {
            ShinyHost
                .Resolve<IConnectivity>()
                .WhenInternetStatusChanged()
                .ObserveOn(RxApp.MainThreadScheduler)
                .ToPropertyEx(this, x => x.IsInternetAvailable)
                .DisposeWith(this.DeactivateWith);
        }
        public virtual void Destroy() => this.DestroyWith?.Dispose();
        public virtual Task<bool> CanNavigateAsync(INavigationParameters parameters) => Task.FromResult(true);

        [Reactive] public bool IsBusy { get; set; }
        [Reactive] public string? Title { get; protected set; }
        public bool IsInternetAvailable { [ObservableAsProperty] get; }


        protected void BindBusyCommand(ICommand command)
            => this.BindBusyCommand((IReactiveCommand)command);


        protected void BindBusyCommand(IReactiveCommand command) =>
            command.IsExecuting.Subscribe(
                x => this.IsBusy = x,
                _ => this.IsBusy = false,
                () => this.IsBusy = false
            )
            .DisposeWith(this.DeactivateWith);


        protected virtual void RememberUserState()
        {
            var binder = ShinyHost.Resolve<IObjectStoreBinder>();
            binder.Bind(this);

            this.DestroyWith.Add(Disposable.Create(() =>
                binder.UnBind(this)
            ));
        }


        public virtual string this[string key] => this.Localize[key];
        public virtual string TranslateEnum<T>(T value) where T : Enum => this.Localize.GetEnum(value);


        ILogger? logger;
        protected ILogger Logger
        {
            get
            {
                this.logger ??= ShinyHost.LoggerFactory.CreateLogger(this.GetType().AssemblyQualifiedName);
                return this.logger;
            }
            set => this.logger = value;
        }


        IDialogs? dialogs;
        public IDialogs Dialogs
        {
            get
            {
                this.dialogs ??= ShinyHost.Resolve<IDialogs>();
                return this.dialogs;
            }
            protected set => this.dialogs = value;
        }


        ILocalize? localize;
        public ILocalize Localize
        {
            get
            {
                this.localize ??= ShinyHost.Resolve<ILocalize>();
                return this.localize;
            }
            protected set => this.localize = value;
        }
    }
}
