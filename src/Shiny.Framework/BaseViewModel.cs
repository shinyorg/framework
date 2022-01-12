using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Extensions.Logging;
using Prism.Navigation;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Shiny.Extensions.Dialogs;
using Shiny.Extensions.Localization;
using Shiny.Net;
using Shiny.Stores;

namespace Shiny
{
    public abstract class BaseViewModel : ReactiveObject, IDestructible
    {
        protected BaseViewModel()
        {
            ShinyHost
                .Resolve<IConnectivity>()
                .WhenInternetStatusChanged()
                .ObserveOn(RxApp.MainThreadScheduler)
                .ToPropertyEx(this, x => x.IsInternetAvailable)
                .DisposeWith(this.DestroyWith);
        }


        private bool _isBusy;
        public bool IsBusy
        {
            get { return _isBusy; }
            set { this.RaiseAndSetIfChanged(ref _isBusy, value); }
        }

        private string? _title;
        public string? Title
        {
            get { return _title; }
            protected set
            {
                this.RaiseAndSetIfChanged(ref _title, value);
            }
        }
        public bool IsInternetAvailable { [ObservableAsProperty] get; }


        CompositeDisposable? deactivateWith;
        protected internal CompositeDisposable DeactivateWith => this.deactivateWith ??= new CompositeDisposable();


        CompositeDisposable? destroyWith;
        protected internal CompositeDisposable DestroyWith => this.destroyWith ??= new CompositeDisposable();


        CancellationTokenSource? deactiveToken;
        protected CancellationToken DeactivateToken
        {
            get
            {
                this.deactiveToken ??= new CancellationTokenSource();
                return this.deactiveToken.Token;
            }
        }

        CancellationTokenSource? destroyToken;
        protected CancellationToken DestroyToken
        {
            get
            {
                this.destroyToken ??= new CancellationTokenSource();
                return this.destroyToken.Token;
            }
        }

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


        ILocalizationManager? localize;
        public ILocalizationManager LocalizationManager
        {
            get
            {
                this.localize ??= ShinyHost.Resolve<ILocalizationManager>();
                return this.localize;
            }
            protected set => this.localize = value;
        }


        public ILocalizationSource? Localize { get; private set; }
        protected void SetLocalization(string section)
            => this.Localize = this.LocalizationManager.GetSection(section);


        protected virtual void Deactivate()
        {
            this.deactiveToken?.Cancel();
            this.deactiveToken?.Dispose();
            this.deactiveToken = null;

            this.deactivateWith?.Dispose();
            this.deactivateWith = null;
        }


        public virtual void Destroy()
        {
            this.destroyToken?.Cancel();
            this.destroyToken?.Dispose();

            this.Deactivate();
            this.destroyWith?.Dispose();
        }





        protected ICommand LoadingDialogCommand(
            Func<Task> action,
            string loadingText = "Loading...",
            bool useSnackbar = false,
            IObservable<bool>? canExecute = null
        ) => ReactiveCommand.CreateFromTask(() => this.Dialogs.LoadingTask(action, loadingText, useSnackbar), canExecute);


        protected virtual void RememberUserState()
        {
            var binder = ShinyHost.Resolve<IObjectStoreBinder>();
            binder.Bind(this);

            this.DestroyWith.Add(Disposable.Create(() =>
                binder.UnBind(this)
            ));
        }


        public virtual string? this[string key]
        {
            get
            {
                string value = "";
                if (this.Localize == null)
                    value = this.LocalizationManager[key];
                else
                    value = this.Localize[key];

                return value;
            }
        }


        protected void BindBusyCommand(ICommand command)
            => this.BindBusyCommand((IReactiveCommand)command);


        protected void BindBusyCommand(IReactiveCommand command) =>
            command.IsExecuting.Subscribe(
                x => this.IsBusy = x,
                _ => this.IsBusy = false,
                () => this.IsBusy = false
            )
            .DisposeWith(this.DeactivateWith);

    }
}
