using System;
using DryIoc;
using Prism.DryIoc;
using Prism.Ioc;
using Prism.Mvvm;


namespace Shiny
{
    public abstract class FrameworkApplication : PrismApplication
    {
        protected override void OnInitialized()
        {
            base.OnInitialized();
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
    }
}
