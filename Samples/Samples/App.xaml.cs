using System;

namespace Samples
{
    public partial class App : Shiny.FrameworkApplication
    {
        protected override void Initialize()
        {
            XF.Material.Forms.Material.Init(this);
            this.InitializeComponent();
            base.Initialize();
        }
    }
}
