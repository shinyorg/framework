using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using ReactiveUI;


namespace Shiny
{
    public enum ErrorAlertType
    {
        None,
        NoLocalize,
        FullError,
        Localize
    }


    public class GlobalExceptionHandlerConfig
    {
        public static GlobalExceptionHandlerConfig Instance { get; } = new GlobalExceptionHandlerConfig();

        public ErrorAlertType AlertType { get; set; } = ErrorAlertType.NoLocalize;
        public string LocalizeErrorTitleKey { get; set; } = "Error";
        public string LocalizeErrorBodyKey { get; set; } = "ErrorDetail";
        public bool IgnoreTokenCancellations { get; set; } = true;
        public bool LogError { get; set; } = true;
    }


    public class GlobalExceptionHandler : IObserver<Exception>, IShinyStartupTask
    {
        readonly ILocalize? localize;
        readonly IDialogs? dialogs;
        readonly ILogger logger;


        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger,
                                      IDialogs? dialogs = null,
                                      ILocalize? localize = null)
        {
            this.dialogs = dialogs;
            this.logger = logger;
            this.localize = localize;
        }


        public void Start() => RxApp.DefaultExceptionHandler = this;
        public void OnCompleted() { }
        public void OnError(Exception error) { }


        protected virtual bool ShouldIgnore(GlobalExceptionHandlerConfig cfg, Exception value)
        {
            if (cfg.IgnoreTokenCancellations && value is TaskCanceledException)
                return true;

            return false;
        }


        public async void OnNext(Exception value)
        {
            var cfg = GlobalExceptionHandlerConfig.Instance;
            if (this.ShouldIgnore(cfg, value))
                return;

            if (cfg.LogError)
                this.logger.LogError(value, "Error in view");

            if (cfg.AlertType != ErrorAlertType.None)
            {
                if (this.dialogs == null)
                    throw new ArgumentException("No dialogs registered");

                var title = "ERROR";
                var body = "There was an error";

                switch (cfg.AlertType)
                {
                    case ErrorAlertType.FullError:
                        body = value.ToString();
                        break;

                    case ErrorAlertType.Localize:
                        if (this.localize == null)
                            throw new ArgumentException("Localize is not registered");

                        title = this.localize[cfg.LocalizeErrorTitleKey];
                        body = this.localize[cfg.LocalizeErrorBodyKey];
                        break;
                }
                await this.dialogs.Alert(body, title);
            }
        }
    }
}