using Microsoft.Extensions.DependencyInjection;
using Shiny.Impl;


namespace Shiny
{
    public class FrameworkStartup : ShinyStartup
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IDialogs, XfMaterialDialogs>();
            services.AddSingleton<GlobalExceptionHandler>();
        }
    }
}
