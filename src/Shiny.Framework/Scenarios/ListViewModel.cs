using Prism.Navigation;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows.Input;


namespace Shiny.Scenarios
{
    public abstract class ListViewModel<T> : ViewModel where T : class
    {
        protected ListViewModel()
        {
            Load = ReactiveCommand.CreateFromTask(Process);
            BindBusyCommand(Load);

            this.WhenAnyValue(x => x.SelectedItem)
                .WhereNotNull()
                .Subscribe(x =>
                {
                    SelectedItem = null;
                    OnSelectedItem(x);
                })
                .DisposeWith(DestroyWith);
        }


        protected virtual async Task Process() =>
            List = await GetData(parameters);


        protected virtual void OnSelectedItem(T item) { }

        protected abstract Task<List<T>> GetData(INavigationParameters parameters);
        public ICommand Load { get; }
        [Reactive] public List<T> List { get; protected set; }
        [Reactive] public T? SelectedItem { get; set; }

        private INavigationParameters parameters;
        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            this.parameters = parameters;
            Load.Execute(null);
        }
    }
}
