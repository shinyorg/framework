using System;
using Microsoft.Extensions.Logging;
using Shiny.Extensions.Localization;
using Shiny.Stores;

namespace Shiny;


public record BaseServices(
    IObjectStoreBinder ObjectBinder,
    IDialogs Dialogs,
    IConnectivity Connectivity,
    ILoggerFactory LoggerFactory,
    IValidationService? Validation,
    ILocalizationManager? localizeManager,
    ILocalizationSource? Localize
);