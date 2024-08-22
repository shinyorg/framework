namespace Shiny.Applications;



public interface ILifecycle
{
    Task<bool> CanNavigateAway();

    //void OnAppearing();
    //void OnDisappearing();

    Task OnNavigatedTo();
    Task OnNavigatedFrom();

    // TODO: what about a task cancellation token or composite disposable
    void OnDestroy();
}


//public interface INavigationArguments<T>
//{
//    // fires while previous page is still active
//    Task Initialize(T args);

//    // fires when entering page
//    Task OnNavigatedTo(T args);
//    //Task OnNavigatedFrom(T args);

//}