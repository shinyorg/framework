using System;
using System.Threading.Tasks;
using Prism.DryIoc;
using Prism.Ioc;
using Prism.Navigation;


namespace Shiny
{
    public abstract class FrameworkApplication : PrismApplication
    {
        protected override async void OnInitialized()
        {
            XF.Material.Forms.Material.Init(this);
            await FrameworkStartup.Current.RunApp(this.NavigationService);
        }


        protected override void RegisterTypes(IContainerRegistry containerRegistry)
            => FrameworkStartup.Current.ConfigureApp(containerRegistry);
    }
}
