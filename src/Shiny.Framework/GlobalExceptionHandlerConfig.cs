namespace Shiny;

public enum ErrorAlertType
{
    None,
    NoLocalize,
    FullError,
    Localize
}


public record GlobalExceptionHandlerConfig(
    ErrorAlertType AlertType = ErrorAlertType.NoLocalize,
    string? LocalizeErrorTitleKey = null,
    string? LocalizeErrorBodyKey = null,
    bool IgnoreTokenCancellations = true,
    bool LogError = true
);