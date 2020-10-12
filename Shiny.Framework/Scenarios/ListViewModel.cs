using System;
using System.Collections.Generic;
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



        protected virtual void OnError(Exception ex) {}
        protected abstract Task<List<T>> GetData();
        public ICommand Load { get; }
        [Reactive] public List<T> List { get; protected set; }


        public override void OnAppearing()
        {
            base.OnAppearing();
            ((ICommand)this.List).Execute(null);
        }
    }
}
