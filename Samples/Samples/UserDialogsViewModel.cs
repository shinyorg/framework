using System;
using System.Collections.Generic;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Shiny;
using Shiny.UserDialogs;


namespace Samples
{
    public class UserDialogsViewModel : TabViewModel
    {
        public UserDialogsViewModel(IUserDialogs dialogs)
        {
            this.Actions = new List<ActionItemViewModel>
            {
                this.Create("ActionSheet", async vm =>
                {
                    vm.Detail = await dialogs
                        .ActionSheet(new ActionSheetOptions
                        {
                            DismissText = "Cancel",
                            Options =
                            {
                                "1",
                                "2",
                                "3"
                            }
                        })
                        .ToTask();
                }),
                this.Create("Alert", async vm =>
                {
                    await dialogs.Alert(new AlertOptions { Title = "Test", Message = "BOO" }).ToTask();
                }),
                this.Create("Confirm", async vm =>
                {

                }),
                this.Create("Prompt", async vm =>
                {

                })
            };
            this.WhenAnyValueSelected(
                x => x.SelectedAction,
                x => x.Command?.Execute(null)
            );
        }


        [Reactive] public ActionItemViewModel SelectedAction { get; set; }
        public List<ActionItemViewModel> Actions { get; }


        ActionItemViewModel Create(string text, Func<ActionItemViewModel, Task> task)
        {
            var vm = new ActionItemViewModel
            {
                Text = text
            };
            vm.Command = ReactiveCommand.CreateFromTask(async () => await task(vm));
            return vm;
        }
    }
}
