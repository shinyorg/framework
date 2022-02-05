using System;
using System.Threading.Tasks;
using DryIoc.Microsoft.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Prism.DryIoc;
using Prism.Ioc;
using Prism.Navigation;
using Shiny.Extensions.Dialogs;
using Shiny.Impl;
using Xamarin.Forms;


namespace Shiny
{
    public abstract class FrameworkStartup : ShinyStartup
    {
        internal static FrameworkStartup? Current { get; private set; }

        protected IPlatform? Platform { get; private set; }
        ILoggingBuilder? builder;


        /// <summary>
        /// This is when the application is ready to run - use the naviagator to navigate to the initial page
        /// </summary>
        /// <param name="navigator"></param>
        /// <returns></returns>
        public abstract Task RunApp(INavigationService navigator);

        /// <summary>
        /// Configure all of your viewmodels and navigation here as well as any foreground only services
        /// </summary>
        /// <param name="containerRegistry"></param>
        /// <param name="app"></param>
        public abstract void ConfigureApp(Application app, IContainerRegistry containerRegistry);

        /// <summary>
        /// Configure your Shiny/Background services and logging here
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="services"></param>
        protected abstract void Configure(ILoggingBuilder builder, IServiceCollection services);


        public override void ConfigureLogging(ILoggingBuilder builder, IPlatform platform)
            => this.builder = builder;


        public override void ConfigureServices(IServiceCollection services, IPlatform platform)
        {
            Current = this;
            this.Platform = platform;
            this.Configure(this.builder!, services);
            services.TryAddSingleton<IDialogs, NativeDialogs>();
            services.TryAddSingleton<IValidationService, DataAnnotationsValidationService>();
        }


        public override IServiceProvider CreateServiceProvider(IServiceCollection services)
        {
            ContainerLocator.SetContainerExtension(() => new DryIocContainerExtension());
            var container = ContainerLocator.Container.GetContainer();
            DryIocAdapter.Populate(container, services);
            return container.GetServiceProvider();
        }


        ///// <summary>
        ///// Call this if your app uses XAML and you don't have it calling InitializeComponent internally
        ///// </summary>
        ///// <param name="app"></param>
        //protected void InitializeApp(Application app)
        //{
        //    global::Xamarin.Forms.Xaml.Extensions.LoadFromXaml(app, app.GetType());
        //}
    }
}
