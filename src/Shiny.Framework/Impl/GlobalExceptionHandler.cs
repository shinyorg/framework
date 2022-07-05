using Microsoft.Extensions.Logging;
using ReactiveUI;
using Shiny.Extensions.Localization;

namespace Shiny.Impl;


public class GlobalExceptionHandler : IObserver<Exception>, IShinyStartupTask
{
    readonly ILocalizationManager? localize;
    readonly IDialogs dialogs;
    readonly ILogger logger;


    public GlobalExceptionHandler(
        ILogger<GlobalExceptionHandler> logger,
        IDialogs dialogs,
        ILocalizationManager? localize = null
    )
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