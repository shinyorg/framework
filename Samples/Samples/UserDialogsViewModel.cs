using System.Collections.Generic;
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
                new ActionItemViewModel
                {
                    Text = ""
                }
            };
            this.WhenAnyValueSelected(
                x => x.SelectedAction,
                x => x.Command?.Execute(null)
            );
        }


        [Reactive] public ActionItemViewModel SelectedAction { get; set; }
        public List<ActionItemViewModel> Actions { get; }
    }
}
