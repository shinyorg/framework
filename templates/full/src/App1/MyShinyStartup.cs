using System;
using App1.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Shiny;


namespace App1
{
    public class MyShinyStartup : FrameworkStartup
    {
        public override void ConfigureLogging(ILoggingBuilder builder, IPlatform platform)
        {
            base.ConfigureLogging(builder, platform);
            builder.AddAppCenter("");
        }


        public override void ConfigureServices(IServiceCollection services, IPlatform platform)
        {
            base.ConfigureServices(services, platform);
            services.RegisterModule(new DataModule());
            services.UseResxLocalization(this.GetType().Assembly, "App1.Resources.Strings.resx");
        }
    }
}
