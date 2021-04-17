using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shiny.Msal;


namespace Shiny
{
    public static class ServiceCollectionExtensions
    {
        public static void UseMsal(this IServiceCollection services, MsalConfiguration configuration)
        {
            services.AddSingleton(configuration);
            services.TryAddSingleton<IMsalService, MsalService>();
            services.AddSingleton<MsalAuthHttpClientHandler>();
        }
    }
}
