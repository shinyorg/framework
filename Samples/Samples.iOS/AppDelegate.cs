using System;
using Foundation;
using Xamarin.Forms.Platform.iOS;

[assembly: Shiny.ShinyApplication(
    ShinyStartupTypeName = "Samples.SampleStartup",
    XamarinFormsAppTypeName = "Samples.App"
)]


namespace Samples.iOS
{
    [Register("AppDelegate")]
    public partial class AppDelegate : FormsApplicationDelegate
    {
    }
}
