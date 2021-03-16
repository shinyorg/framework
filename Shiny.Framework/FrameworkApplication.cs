using System;
using Prism.DryIoc;


namespace Shiny
{
    public abstract class FrameworkApplication : PrismApplication
    {
        protected override void OnInitialized()
        {
            XF.Material.Forms.Material.Init(this);
        }
    }
}
