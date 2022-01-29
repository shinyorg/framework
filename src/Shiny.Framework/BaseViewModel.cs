using Microsoft.Extensions.Logging;
using Prism.Navigation;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Shiny.Extensions.Dialogs;
using Shiny.Extensions.Localization;
using Shiny.Net;
using Shiny.Stores;
using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

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
                .DisposeWith(DestroyWith);
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

        private CompositeDisposable? deactivateWith;
        protected internal CompositeDisposable DeactivateWith => deactivateWith ??= new CompositeDisposable();

        private CompositeDisposable? destroyWith;
        protected internal CompositeDisposable DestroyWith => destroyWith ??= new CompositeDisposable();

        private CancellationTokenSource? deactiveToken;
        protected CancellationToken DeactivateToken
        {
            get
            {
                deactiveToken ??= new CancellationTokenSource();
                return deactiveToken.Token;
            }
        }

        private CancellationTokenSource? destroyToken;
        protected CancellationToken DestroyToken
        {
            get
            {
                destroyToken ??= new CancellationTokenSource();
                return destroyToken.Token;
            }
        }

        private ILogger? logger;
        protected ILogger Logger
        {
            get
            {
                logger ??= ShinyHost.LoggerFactory.CreateLogger(GetType().AssemblyQualifiedName);
                return logger;
            }
            set => logger = value;
        }

        private IDialogs? dialogs;
        public IDialogs Dialogs
        {
            get
            {
                dialogs ??= ShinyHost.Resolve<IDialogs>();
                return dialogs;
            }
            protected set => dialogs = value;
        }

        private ILocalizationManager? localize;
        public ILocalizationManager LocalizationManager
        {
            get
            {
                localize ??= ShinyHost.Resolve<ILocalizationManager>();
                return localize;
            }
            protected set => localize = value;
        }


        public ILocalizationSource? Localize { get; private set; }
        protected void SetLocalization(string section)
            => Localize = LocalizationManager.GetSection(section);


        protected virtual void Deactivate()
        {
            deactiveToken?.Cancel();
            deactiveToken?.Dispose();
            deactiveToken = null;

            deactivateWith?.Dispose();
            deactivateWith = null;
        }


        public virtual void Destroy()
        {
            destroyToken?.Cancel();
            destroyToken?.Dispose();

            Deactivate();
            destroyWith?.Dispose();
        }





        protected ICommand LoadingDialogCommand(
            Func<Task> action,
            string loadingText = "Loading...",
            bool useSnackbar = false,
            IObservable<bool>? canExecute = null
        ) => ReactiveCommand.CreateFromTask(() => Dialogs.LoadingTask(action, loadingText, useSnackbar), canExecute);


        protected virtual void RememberUserState()
        {
            var binder = ShinyHost.Resolve<IObjectStoreBinder>();
            binder.Bind(this);

            DestroyWith.Add(Disposable.Create(() =>
                binder.UnBind(this)
            ));
        }


        public virtual string? this[string key]
        {
            get
            {
                string value = "";
                if (Localize == null)
                    value = LocalizationManager[key];
                else
                    value = Localize[key];

                return value;
            }
        }


        protected void BindBusyCommand(ICommand command)
            => BindBusyCommand((IReactiveCommand)command);


        protected void BindBusyCommand(IReactiveCommand command) =>
            command.IsExecuting.Subscribe(
                x => IsBusy = x,
                _ => IsBusy = false,
                () => IsBusy = false
            )
            .DisposeWith(DeactivateWith);

    }
}
