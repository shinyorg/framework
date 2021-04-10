using System;
using DryIoc;
using DryIoc.Microsoft.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Prism.DryIoc;
using Prism.Ioc;


namespace Shiny
{
    public abstract class FrameworkStartup : ShinyStartup
    {
        protected IPlatform Platform { get; private set; }
        ILoggingBuilder? builder;


        public override void ConfigureLogging(ILoggingBuilder builder, IPlatform platform)
            => this.builder = builder;



        public override void ConfigureServices(IServiceCollection services, IPlatform platform)
        {
            this.Platform = platform;
            this.Configure(this.builder!, services);
        }


        protected abstract void Configure(ILoggingBuilder builder, IServiceCollection services);


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
