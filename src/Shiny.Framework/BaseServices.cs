using Microsoft.Extensions.Logging;
using Shiny.Net;
using Shiny.Stores;
using Microsoft.Extensions.Localization;

namespace Shiny;


public record BaseServices(
    #if PLATFORM
    IPlatform Platform,
    #endif
    INavigationService Navigation,
    IDialogs Dialogs,
    IObjectStoreBinder ObjectBinder,
    GlobalExceptionAction ErrorHandler,
    IConnectivity Connectivity,
    ILoggerFactory LoggerFactory,
    IValidationService? Validation = null, 
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