using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Shiny;


namespace Samples
{
    public class SampleStartup : FrameworkStartup
    {
        protected override void Configure(ILoggingBuilder builder, IServiceCollection services)
        {
            services.UseGlobalCommandExceptionHandler();

            services.UseMsal(new MsalConfiguration(
                Secrets.Values.MsalClientId,
                Secrets.Values.MsalScopes.Split(';')
            )
            {
                //TenantId = Secrets.Values.MsalTenantId,
                //UseBroker = true,
                //SignatureHash = Secrets.Values.MsalSignatureHash
            });
        }
    }
}
