using System;
using Foundation;

[assembly: Shiny.ShinyApplication(
    ShinyStartupTypeName = "App1.MyShinyStartup",
    XamarinFormsAppTypeName = "App1.App"
)]

namespace App1.iOS
{
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
    }
}