using Microsoft.Identity.Client;


namespace Shiny.Msal
{
    public class MsalConfiguration
    {
        public string? AppId { get; set; }
        public string? ClientId { get; set; }
        public string[]? Scopes { get; set; }
        public string? Authority { get; set; }
        public Prompt? LoginPrompt { get; set; }
        public bool? UseEmbeddedWebView { get; set; }
        public string? SignatureHash { get; set; }
    }
}
