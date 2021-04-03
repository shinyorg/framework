using System;
using Microsoft.Extensions.DependencyInjection;
using Shiny;


namespace App1.Data
{
    public class DataModule : ShinyModule
    {
        public override void Register(IServiceCollection services)
        {
            services.AddSingleton<IAuthService, AuthService>();
        }
    }
}
