# Shiny Framework Libraries

These libraries were written by the author for the author and thus are not open to issues, enhancements, or anything in between.

## Shiny.Framework
Framework combines the best of MVVM using Prism & ReactiveUI while also combining Shiny.  Find the sample at [https://github.com/shinyorg/samples/tree/main/Integration-Best-Prism-RXUI]


#### Features
* Simplified configuration - Prism & Shiny setup under one file using FrameworkStartup
* No guess work about what dependency injection mechanism to install - Framework uses [DryIoc](https://github.com/dadhi/DryIoc) under the hood, but guess what.... you'll NEVER know it even if this changes one day
* Dialogs - a pretty API that brings together [XF Material](https://github.com/Baseflow/XF-Material-Library) (built on [RG Popups] (https://github.com/rotorgames/Rg.Plugins.Popup))
* Localization - a simple to use localization framework that can be used everywhere including your viewmodels!  Localization DONE RIGHT!
* Global Navigator - allows you to inject a global navigator that you can use safely from your Shiny delegates.  Will ignore navigation requests when in the background
* Global Command Exception Handler - do you like ReactiveCommand, so do I... this little service brings together Shiny's logging services + localization (from above) + dialogs (also from above) into one singular place
* ViewModel Validation using Data Annotations by default (but can be changed to 3rd party by implementing IValidationService)

## Nugets

|Release|NuGet|
|-------|-----|
|Stable|![Nuget](https://img.shields.io/nuget/v/shiny.framework?style=for-the-badge)|
|Preview|![Nuget (with prereleases)](https://img.shields.io/nuget/vpre/shiny.framework?style=for-the-badge)|


## Builds

|Branch|Status|
|------|------|
|Master|[![Build](https://github.com/shinyorg/framework/actions/workflows/build.yml/badge.svg)](https://github.com/shinyorg/framework/actions/workflows/build.yml)|
|Dev|[![Build](https://github.com/shinyorg/framework/actions/workflows/build.yml/badge.svg?branch=dev)](https://github.com/shinyorg/framework/actions/workflows/build.yml)|
|Preview|[![Build](https://github.com/shinyorg/framework/actions/workflows/build.yml/badge.svg?branch=preview)](https://github.com/shinyorg/framework/actions/workflows/build.yml)|


## Wiring Up (Prism Plus Shiny)

* First install Shiny.Framework from NuGet
* Setup your App.xaml like so (NOTE: you can add resources like normal):

```xml
<?xml version="1.0" encoding="utf-8" ?>
<shiny:FrameworkApplication xmlns="http://xamarin.com/schemas/2014/forms"
                            xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
                            xmlns:shiny="clr-namespace:Shiny;assembly=Shiny.Framework"
                            x:Class="Samples.App">
</shiny:FrameworkApplication>
```

* add the following to your app.xaml.cs

```csharp
using System;

namespace YourNamespace
{
    public partial class App : Shiny.FrameworkApplication
    {
        protected override void Initialize()
        {
            XF.Material.Forms.Material.Init(this); // if you are using XF Material dialogs
            this.InitializeComponent();
            base.Initialize();
        }
    }
}
```

* Now you are ready to implement a Shiny Startup/Prism Definition.  Here is an example:
```csharp
namespace YourNamespace
{
    public class YourStartupClassName : Shiny.FrameworkStartup
    {
        protected override void Configure(ILoggingBuilder builder, IServiceCollection services)
        {
            // any services you register with Prism or Shiny should go here
        }

        // your start page so Prism knows where to go
        public override Task RunApp(INavigationService navigator)
            => navigator.Navigate("NavigationPage/MainPage"); 

        // register your Prism pages
        public override void ConfigureApp(Application app, IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<OtherPage, OtherViewModel>();
        }
    }
}
```


## The Ultimate In Base ViewModels

Inherit Shiny.ViewModel on your viewmodel.  You will instantly gain the following:

* Features
    * Dialogs is built right in
    * Localization is built right in (you need to register it though, we will cover that in the localization section)
    * All of the Prism lifecycle stuff waiting to be used (just type override to see everything)
    * All of the Reactive setup (WhenAny, WhenAnyValue) built right in
    * Low Effort Validation

## Localization

This uses Shiny.Extensions.Localization - go to <https://shinylib.net/apiservices> for more info on this library

```csharp
namespace YourNamespace
{
    public class YourStartupClassName : Shiny.FrameworkStartup
    {
        protected override void Configure(ILoggingBuilder builder, IServiceCollection services)
        {
            var manager = new LocalizationBuilder()
                .AddResource("Samples.Resources.Strings", this.GetType().Assembly, "Strings")
                .AddResource("Samples.Resources.Enums", this.GetType().Assembly, "Enums")
                .Build();

            services.AddSingleton(manager); // ILocalizationManager if you need this in other services
        }
    }
}
```

## Dialogs

We recommend installing Shiny.Extensions.Dialogs.XfMaterial for a full experience.  However, if you don't, we use standard native dialogs under the hood.  However, you will be missing loading & snackbar with the default implementation.

To use XF Material Dialogs

```csharp
public class SampleStartup : FrameworkStartup
{
    protected override void Configure(ILoggingBuilder builder, IServiceCollection services)
    {
        services.UseXfMaterialDialogs();
    }
}
```

For examples on what methods are available in dialogs, take a look here: [Dialog Samples](https://github.com/shinyorg/framework/blob/master/Samples/Samples/DialogsViewModel.cs)

## Global Command Exception Handler

Global command exception handling is something provided by ReactiveUI.  It saves you the hassle of putting a standard try {} catch (Exception ex) {} trap all in every command.

Shiny builds on this by adding logging and a standard dialog message to handle trap-all scenarios.  To use it, simply add the following to your framework startup:

```csharp
namespace YourNamespace
{
    public class YourStartupClassName : Shiny.FrameworkStartup
    {
        protected override void Configure(ILoggingBuilder builder, IServiceCollection services)
        {
            services.UseGlobalCommandExceptionHandler(options =>
            {
#if DEBUG
                options.AlertType = ErrorAlertType.FullError; // this will show the full error in a dialog during debug
                options.LogError = false;
#else
                // you will need localization to be registered to use this
                options.AlertType = ErrorAlertType.Localize; // you can use NoLocalize which will send a standard english message instead
                options.LogError = true;
                options.IgnoreTokenCancellations = true;
                options.LocalizeErrorTitleKey = "YourLocalizationTitleKey";
                options.LocalizeErrorBodyKey = "YourLocalizationErrorBodyMessageKey";
#endif
            });
        }
    }
}
```


## ViewModel Validation

Validation is often the painful act of doing the same thing over and over and over.  There are tools out there that help such as FluentValidation and Data Annotations, but nothing out of the box.

Shiny.Framework now includes optional support for DataAnnotations (with a pluggable model under IValidationService); 

Data Annotations is currently the only out-of-box provider for validation.  Take a look at [Microsoft Data Annotations](https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.dataannotations?view=net-6.0) for more documentation on Data Annotations


#### Setup

NOTE: we recommend usage of [ReactiveUI.Fody](https://www.nuget.org/packages/ReactiveUI.Fody/) for the use of the [Reactive] attribute which removes the boilerplate code for MVVM properties

First, we need to tell Shiny.Framework to wire in the validation service to Data Annotations by doing the following in your shiny startup

```csharp
public class SampleStartup : Shiny.FrameworkStartup
{
    protected override void Configure(ILoggingBuilder builder, IServiceCollection services)
    {
        services.UseDataAnnotationValidation();
    }
}
    
```


Now, in your viewmodel:

```csharp
public class MyViewModel : Shiny.ViewModel
{
    [Reactive]
    [EmailAddress(ErrorMessage = "Invalid Email")]
    public string Email { get; set; }


    [Reactive]
    [MinLength(3, ErrorMessage = "Min 3 Characters")]
    [MaxLength(5, ErrorMessage = "Max 5 Characters")]
    public string LengthTest { get; set; }
}
```

####

#### Commands
```csharp

public class MyViewModel : Shiny.ViewModel
{
    public MyViewModel() {
        this.Command = ReactiveCommand.Create(() => {}, this.WhenValid());
    }

    public ICommand Command { get; }

    // add some data annotated MVVM properties below
}
```

#### Localization

Data annotations actually stinks for pluggable localization.  Shiny is big on pluggability so we've "hacked" our way around this one.  Simply register your localization manager and follow this slight hack

We recommand creating a secondary section called Validation.  Take a look at the sample app within this repo for examples.

```csharp
public class MyViewModel : Shiny.ViewModel
{
    [Reactive]
    [EmailAddress(ErrorMessage = "localize:Validation:Required")]
    public string Email { get; set; }
}
```

> NOTE: the use of "localize:" if front the localization key.  Also note, that uses the fully qualified path to your localization value (ie Section:Key)
