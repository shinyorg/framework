using System;
using Prism.DryIoc;
using Prism.Ioc;


namespace Shiny
{
    public abstract class FrameworkApplication : PrismApplication
    {
        protected override void OnInitialized()
        {
            XF.Material.Forms.Material.Init(this);
        }


        protected override IContainerExtension CreateContainerExtension()
            => new DryIocContainerExtension(FrameworkStartup.Container);
    }
}
