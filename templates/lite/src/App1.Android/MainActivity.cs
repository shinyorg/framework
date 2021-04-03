using System;
using Android.App;
using Android.Content.PM;

[assembly: Shiny.ShinyApplication(
    ShinyStartupTypeName = "App1.MyShinyStartup",
    XamarinFormsAppTypeName = "App1.App"
)]


namespace App1.Droid
{
    [Activity(
        Label = "App1",
        Icon = "@mipmap/icon",
        Theme = "@style/MainTheme",
        MainLauncher = true,
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize
    )]
    public partial class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
    }
}