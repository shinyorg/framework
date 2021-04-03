using System;
using DryIoc;
using DryIoc.Microsoft.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Prism.DryIoc;
using Prism.Ioc;


namespace Shiny
{
    public class FrameworkStartup : ShinyStartup
    {
        public override void ConfigureServices(IServiceCollection services, IPlatform platform)
        {
            services.UseXfMaterialDialogs();
            services.UseGlobalCommandExceptionHandler();
        }


        internal static IContainer? Container { get; private set; }
        public override IServiceProvider CreateServiceProvider(IServiceCollection services)
        {
            ContainerLocator.SetContainerExtension(() => new DryIocContainerExtension());
            var container = ContainerLocator.Container.GetContainer();
            DryIocAdapter.Populate(container, services);
            return container.GetServiceProvider();
        }
    }
}
