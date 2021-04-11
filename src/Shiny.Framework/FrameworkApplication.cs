using System;
using System.Threading.Tasks;
using Prism.DryIoc;
using Prism.Navigation;


namespace Shiny
{
    public abstract class FrameworkApplication : PrismApplication
    {
        protected abstract Task Run(INavigationService navigator);


        protected override async void OnInitialized()
        {
            XF.Material.Forms.Material.Init(this);
            await this.Run(this.NavigationService);
        }
    }
}
