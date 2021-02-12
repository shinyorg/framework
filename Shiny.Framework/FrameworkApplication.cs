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
            XF.Material.Forms.Material.Init(this);

            ViewModelLocationProvider.SetDefaultViewTypeToViewModelTypeResolver(viewType =>
            {
                var viewModelTypeName = viewType.FullName.Replace("Page", "ViewModel");
                var viewModelType = Type.GetType(viewModelTypeName);
                return viewModelType;
            });
        }



        protected override IContainerExtension CreateContainerExtension()
            => new DryIocContainerExtension(FrameworkStartup.Container);
    }
}
