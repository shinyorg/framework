using System;
using System.Reactive.Subjects;
using Prism.Navigation;


namespace Shiny
{
    public class ReactiveViewModel : ViewModel
    {
        Subject<INavigationParameters>? navToSubj;
        public IObservable<INavigationParameters> WhenNavigatedTo()
        {
            navToSubj ??= new Subject<INavigationParameters>();
            return navToSubj.DisposedBy(this.DestroyWith);
        }


        Subject<INavigationParameters>? navFromSubj;
        public IObservable<INavigationParameters> WhenNavigatedFrom()
        {
            navFromSubj ??= new Subject<INavigationParameters>();
            return navFromSubj.DisposedBy(this.DestroyWith);
        }


        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            this.navToSubj?.OnNext(parameters);
        }


        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            base.OnNavigatedFrom(parameters);
            this.navFromSubj?.OnNext(parameters);
        }
    }
}
