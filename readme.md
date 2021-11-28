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


## Shiny.Msal
* A helper library that brings MSAL into DI easily
* A great article for setting up MSAL with Xamarin - https://www.thewissen.io/implementing-msal-authentication-in-xamarin-forms/
* https://docs.microsoft.com/en-ca/azure/active-directory/develop/msal-net-xamarin-android-considerations
* https://docs.microsoft.com/en-ca/azure/active-directory/develop/msal-net-xamarin-ios-considerations
* https://docs.microsoft.com/en-ca/azure/active-directory/develop/msal-net-use-brokers-with-xamarin-apps