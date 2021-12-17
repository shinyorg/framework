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
        public string LocalizeErrorTitleKey { get; set; } = "Strings:Error";
        public string LocalizeErrorBodyKey { get; set; } = "Strings:ErrorDetail";
        public bool IgnoreTokenCancellations { get; set; } = true;
        public bool LogError { get; set; } = true;
    }
}