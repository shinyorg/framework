using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Extensions.Logging;
using Prism.Navigation;
using ReactiveUI;
using Shiny.Extensions.Dialogs;
using Shiny.Extensions.Localization;
using Shiny.Net;
using Shiny.Stores;


namespace Shiny
{
    public abstract class BaseViewModel : ReactiveObject, IDestructible, IValidationViewModel
    {
        protected BaseViewModel()
        {
            this.isInternetAvaibale = ShinyHost
                .Resolve<IConnectivity>()
                .WhenInternetStatusChanged()
                .ObserveOn(RxApp.MainThreadScheduler)
                .ToProperty(this, x => x.IsInternetAvailable)
                .DisposeWith(this.DestroyWith);

            this.Localize = ShinyHost.Resolve<ILocalizationSource>(); // try to set the default section if there is one

            var validationService = ShinyHost.Resolve<IValidationService>();
            if (validationService != null)
            {
                this.Validation = validationService.Bind(this);
                this.DestroyWith.Add(this.Validation);
            }
        }


        bool isBusy;
        public bool IsBusy
        {
            get => this.isBusy;
            set => this.RaiseAndSetIfChanged(ref this.isBusy, value);
        }


        string? title;
        public string? Title
        {
            get => this.title;
            protected set => this.RaiseAndSetIfChanged(ref this.title, value);
        }

        readonly ObservableAsPropertyHelper<bool> isInternetAvaibale;
        public bool IsInternetAvailable => this.isInternetAvaibale.Value;
        public IValidationBinding? Validation { get; private set; }


        CompositeDisposable? deactivateWith;
        protected internal CompositeDisposable DeactivateWith => this.deactivateWith ??= new CompositeDisposable();


        CompositeDisposable? destroyWith;
        protected internal CompositeDisposable DestroyWith => this.destroyWith ??= new CompositeDisposable();


        CancellationTokenSource? deactiveToken;
        /// <summary>
        /// The destroy cancellation token - called when your model is deactivated
        /// </summary>
        protected CancellationToken DeactivateToken
        {
            get
            {
                this.deactiveToken ??= new CancellationTokenSource();
                return this.deactiveToken.Token;
            }
        }


        CancellationTokenSource? destroyToken;
        /// <summary>
        /// The destroy cancellation token - called when your model is destroyed
        /// </summary>
        protected CancellationToken DestroyToken
        {
            get
            {
                this.destroyToken ??= new CancellationTokenSource();
                return this.destroyToken.Token;
            }
        }


        ILogger? logger;
        /// <summary>
        /// A lazy loader logger instance for this viewmodel instance
        /// </summary>
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
        /// <summary>
        /// Dialog service from the service provider
        /// </summary>
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
        /// <summary>
        /// Localization manager from the service provider
        /// </summary>
        public ILocalizationManager LocalizationManager
        {
            get
            {
                this.localize ??= ShinyHost.Resolve<ILocalizationManager>();
                return this.localize;
            }
            protected set => this.localize = value;
        }



        /// <summary>
        /// The localization source for this instance - will attempt to use the default section (if registered)
        /// </summary>
        public ILocalizationSource? Localize {  get; protected set; }

        
        /// <summary>
        /// This can be called manually, generally used when your viewmodel is going to the background in the nav stack
        /// </summary>
        protected virtual void Deactivate()
        {
            this.deactiveToken?.Cancel();
            this.deactiveToken?.Dispose();
            this.deactiveToken = null;

            this.deactivateWith?.Dispose();
            this.deactivateWith = null;
        }


        /// <summary>
        /// Called when the viewmodel is being destroyed (not in nav stack any longer)
        /// </summary>
        public virtual void Destroy()
        {
            this.destroyToken?.Cancel();
            this.destroyToken?.Dispose();

            this.Deactivate();
            this.destroyWith?.Dispose();
        }


        /// <summary>
        /// Binds to IsBusy while your command works
        /// </summary>
        /// <param name="command"></param>
        protected void BindBusyCommand(ICommand command)
            => this.BindBusyCommand((IReactiveCommand)command);


        /// <summary>
        /// Binds to IsBusy while your command works
        /// </summary>
        /// <param name="command"></param>
        protected void BindBusyCommand(IReactiveCommand command) =>
            command.IsExecuting.Subscribe(
                x => this.IsBusy = x,
                _ => this.IsBusy = false,
                () => this.IsBusy = false
            )
            .DisposeWith(this.DeactivateWith);


        /// <summary>
        /// Calls the loading task from dialog service while your command works
        /// </summary>
        /// <param name="action"></param>
        /// <param name="loadingText"></param>
        /// <param name="useSnackbar"></param>
        /// <param name="canExecute"></param>
        /// <returns></returns>
        protected ICommand LoadingCommand(
            Func<Task> action,
            string loadingText = "Loading...",
            bool useSnackbar = false,
            IObservable<bool>? canExecute = null
        ) => ReactiveCommand.CreateFromTask(() => this.Dialogs.LoadingTask(action, loadingText, useSnackbar), canExecute);


        /// <summary>
        /// Records the state of this model type for all get/set properties
        /// </summary>
        protected virtual void RememberUserState()
        {
            var binder = ShinyHost.Resolve<IObjectStoreBinder>();
            binder.Bind(this);

            this.DestroyWith.Add(Disposable.Create(() =>
                binder.UnBind(this)
            ));
        }


        /// <summary>
        /// Reads localization key from localization service
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public virtual string? this[string key]
        {
            get
            {
                if (this.LocalizationManager == null)
                    throw new InvalidOperationException("Localization has not been initialized in your DI container");

                if (key.Contains(":") || this.Localize == null)
                    return this.LocalizationManager[key];

                return this.Localize[key];
            }
        }
    }
}
