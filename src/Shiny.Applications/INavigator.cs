namespace Shiny.Applications;

public interface INavigator
{
    // TODO: args
    //Task Navigate(string uri);
    //Task Pop(int backCount = 1);

    // TODO: back to root, viewmodel and uri nav
    // TODO: nav registration
    // Prism - // to reset to root
    // Prism - TabbedPage?SelectedTab=PageName
    // Prism - TabbedPage?CreateTab=PageName
    // Prism - TabbedPage?CreateTab=NavigationPage|PageName
    // Prism - FlyoutPage?Detail=Page
    // Prism - OnStart Route
    // TODO: force startup page for async loading?

    /// <summary>
    /// 
    /// </summary>
    /// <param name="uri"></param>
    /// <returns></returns>
    Task GoTo(string uri);

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    IObservable<object> WhenNavigating();
    // TODO: from page/viewmodel, to page/viewmodel, args, nav type - forward, back, active tab?
}