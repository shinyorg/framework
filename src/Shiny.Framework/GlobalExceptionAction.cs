using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace Shiny;


public class GlobalExceptionAction
{
    readonly GlobalExceptionHandlerConfig config;
    readonly IStringLocalizer? localize;
    readonly IDialogs dialogs;
    readonly ILogger logger;


    public GlobalExceptionAction(
        GlobalExceptionHandlerConfig config,
        ILogger<GlobalExceptionAction> logger,
        IDialogs dialogs,
        IStringLocalizer<GlobalExceptionAction>? localize = null
    )
    {
        this.config = config;
        this.dialogs = dialogs;
        this.logger = logger;
        this.localize = localize;
    }


    protected virtual bool ShouldIgnore(GlobalExceptionHandlerConfig cfg, Exception value)
    {
        if (cfg.IgnoreTokenCancellations && value is TaskCanceledException)
            return true;

        return false;
    }


    public virtual async Task Process(Exception value)
    {
        var cfg = this.config;
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

