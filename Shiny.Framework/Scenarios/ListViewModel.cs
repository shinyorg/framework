using System;
using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;


namespace Shiny.Scenarios
{
    public abstract class ListViewModel<T> : ViewModel
    {
        protected ListViewModel()
        {
            this.Load = ReactiveCommand.CreateFromTask(this.Process);
            this.BindBusyCommand(this.Load);

            this.WhenAnyValue(x => x.SelectedItem)
                .Skip(1)
                .Subscribe(this.OnSelectedItem)
                .DisposeWith(this.DestroyWith);
        }


        protected virtual async Task Process()
        {
            try
            {
                this.List = await this.GetData();
            }
            catch (Exception ex)
            {
                this.OnError(ex);
            }
        }


        protected virtual void OnSelectedItem(T item) {}
        protected virtual void OnError(Exception ex) {}

        protected abstract Task<List<T>> GetData();
        public ICommand Load { get; }
        [Reactive] public List<T> List { get; protected set; }
        [Reactive] public T SelectedItem { get; set; }


        public override void OnAppearing()
        {
            base.OnAppearing();
            ((ICommand)this.List).Execute(null);
        }
    }
}
