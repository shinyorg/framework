using Microsoft.Extensions.Logging;
using Shiny.Extensions.Localization;
using Shiny.Stores;

namespace Shiny;


public record BaseServices(
    #if PLATFORM
    IPlatform Platform,
    #endif
    IDialogs Dialogs,
    IConnectivity Connectivity, // TODO: switch to shiny?
    ILoggerFactory LoggerFactory,
    IObjectStoreBinder ObjectBinder,
    IValidationService? Validation = null,
    ILocalizationManager? localizeManager = null,
    ILocalizationSource? Localize = null
);