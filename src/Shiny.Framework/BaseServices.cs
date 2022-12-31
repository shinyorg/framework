using Microsoft.Extensions.Logging;
using Shiny.Extensions.Localization;
using Shiny.Net;
using Shiny.Impl;
using Shiny.Stores;

namespace Shiny;


public record BaseServices(
    #if PLATFORM
    IPlatform Platform,
    #endif
    INavigationService Navigation,
    IDialogs Dialogs,
    IConnectivity Connectivity,
    ILoggerFactory LoggerFactory,
    IObjectStoreBinder ObjectBinder,
    GlobalExceptionAction ErrorHandler,
    IValidationService? Validation = null,
    ILocalizationManager? LocalizationManager = null,
    ILocalizationSource? Localize = null
);