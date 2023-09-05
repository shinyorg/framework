# Shiny Framework Libraries

These libraries were written by the author for the author and thus are not open to issues, enhancements, or anything in between.

Shiny Framework is not required to run your applications with Shiny.  It is a set of tools designed to make working with Shiny easier.  v2 of Framework does enable a much better integration between Shiny, Prism, & Classic Xamarin applications.  v3 (currently in preview) is only a collection tools.

## Shiny.Framework
Framework combines the best of MVVM using Prism & ReactiveUI while also combining Shiny.  Find the sample at [https://github.com/shinyorg/samples/tree/main/Integration-Best-Prism-RXUI]


#### Features
* Localization - a simple to use localization framework that can be used everywhere including your viewmodels!  Localization DONE RIGHT!
* Global Navigator - allows you to inject a global navigator that you can use safely from your Shiny delegates.  Will ignore navigation requests when in the background
* Global Command Exception Handler - do you like ReactiveCommand, so do I... this little service brings together Shiny's logging services + localization (from above) + dialogs (also from above) into one singular place
* ViewModel Validation using Data Annotations by default (but can be changed to 3rd party by implementing IValidationService)

## Nugets

|Release|NuGet|
|-------|-----|
|Stable|![Nuget](https://img.shields.io/nuget/v/shiny.framework?style=for-the-badge)|
|Preview|![Nuget (with prereleases)](https://img.shields.io/nuget/vpre/shiny.framework?style=for-the-badge)|


## The Ultimate In Base ViewModels

Inherit Shiny.ViewModel on your viewmodel.  You will instantly gain the following:

* Features
    * Dialogs is built right in
    * Localization is built right in (you need to register it though, we will cover that in the localization section)
    * All of the Prism lifecycle stuff waiting to be used (just type override to see everything)
    * All of the Reactive setup (WhenAny, WhenAnyValue) built right in
    * Low Effort Validation

## Localization

We use Microsoft.Extensions.Localization with framework

```csharp

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp
			.CreateBuilder()
            .UseShinyFramework();

        builder.Services.AddLocalization();

		return builder.Build();
	}
}
```

And to bind with it in your XAML UI

```xml
<Label Text="{Binding [MyResourceKey]}" />
```

## Global Command Exception Handler

Global command exception handling is something provided by ReactiveUI.  It saves you the hassle of putting a standard try {} catch (Exception ex) {} trap all in every command.

Shiny builds on this by adding logging and a standard dialog message to handle trap-all scenarios.  To use it, simply add the following to your framework startup:

```csharp
public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp
			.CreateBuilder()
            .UseShinyFramework();

        builder.Services.AddGlobalCommandExceptionHandler(new(
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
```


## ViewModel Validation

Validation is often the painful act of doing the same thing over and over and over.  There are tools out there that help such as FluentValidation and Data Annotations, but nothing out of the box.

Shiny.Framework now includes optional support for DataAnnotations (with a pluggable model under IValidationService); 

Data Annotations is currently the only out-of-box provider for validation.  Take a look at [Microsoft Data Annotations](https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.dataannotations?view=net-6.0) for more documentation on Data Annotations


#### Setup

NOTE: we recommend usage of [ReactiveUI.Fody](https://www.nuget.org/packages/ReactiveUI.Fody/) for the use of the [Reactive] attribute which removes the boilerplate code for MVVM properties

First, we need to tell Shiny.Framework to wire in the validation service to Data Annotations by doing the following in your shiny startup

```csharp
public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp
			.CreateBuilder()
            .UseShinyFramework();

        builder.Services.AddDataAnnotationValidation();
    }
}
    
```


Now, in your viewmodel:

```csharp
public class MyViewModel : Shiny.ViewModel
{
    public MyViewModel()
    {
        this.BindValidation(); 
    }


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
    public MyViewModel()
    {
        this.Command = ReactiveCommand.Create(() => {}, this.WhenValid());
        this.BindValidation();
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
