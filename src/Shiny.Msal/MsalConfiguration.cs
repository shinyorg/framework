using Microsoft.Identity.Client;


namespace Shiny
{
    public class MsalConfiguration
    {
        public MsalConfiguration(string clientId, params string[] scopes)
        {
            this.ClientId = clientId;
            this.Scopes = scopes;
        }


        public bool UseBroker { get; set; }
        public string ClientId { get; }
        public string[] Scopes { get; }
        public string? Authority { get; set; }
        public Prompt? LoginPrompt { get; set; }
        public bool? UseEmbeddedWebView { get; set; }
        public string? SignatureHash { get; set; }
        public string? IosKeychainSecurityGroup { get; set; }
    }
}
