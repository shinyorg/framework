using System;
using DryIoc;
using Microsoft.Extensions.DependencyInjection;
using Prism.DryIoc;
using Prism.Ioc;
using Prism.Mvvm;
using Shiny.Impl;


namespace Shiny
{
    public abstract class ShinyApplication : PrismApplication, IShinyStartup
    {
        protected override void OnInitialized()
        {
            XF.Material.Forms.Material.Init(this);

            ViewModelLocationProvider.SetDefaultViewTypeToViewModelTypeResolver(viewType =>
            {
                var viewModelTypeName = viewType.FullName.Replace("Page", "ViewModel");
                var viewModelType = Type.GetType(viewModelTypeName);
                return viewModelType;
            });
        }


        protected override IContainerExtension CreateContainerExtension()
        {
            var container = new Container(this.CreateContainerRules());
            ShinyHost.Populate((serviceType, func, lifetime) =>
                container.RegisterDelegate(
                    serviceType,
                    _ => func(),
                    Reuse.Singleton // HACK: I know everything is singleton
                )
            );
            return new DryIocContainerExtension(container);
        }


        public virtual void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IDialogs, XfMaterialDialogs>();
            services.AddSingleton<GlobalExceptionHandler>();
        }


        public virtual IServiceProvider CreateServiceProvider(IServiceCollection services) => null;
        public virtual void ConfigureApp(IServiceProvider provider) { }
    }
}
