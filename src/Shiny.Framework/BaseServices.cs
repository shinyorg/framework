using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Localization;
using Prism.Navigation;
using Shiny.Net;

namespace Shiny;


public record BaseServices(
    INavigationService Navigation,
    IDialogs Dialogs,
    IConnectivity Connectivity,
    ILoggerFactory LoggerFactory,
    IStringLocalizerFactory? StringLocalizationFactory = null
);
//{
//    public async Task SafeExecute(Func<Task> task)
//    {
//        try
//        {
//            await task.Invoke().ConfigureAwait(false);
//        }
//        catch (Exception ex)
//        {
//            this.Logger.LogError(ex, "");
//            await this.Dialogs.Alert("", "");
//        }
//    }
//};