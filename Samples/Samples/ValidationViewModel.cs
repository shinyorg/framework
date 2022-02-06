using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Shiny;
using System.ComponentModel.DataAnnotations;
using System.Windows.Input;


namespace Samples
{
    public class ValidationViewModel : BaseViewModel
    {
        public ValidationViewModel()
        {
            this.EnableValidation();
            this.TestCommand = ReactiveCommand.CreateFromTask(
                () => this.Dialogs.Alert("YAY you are valid"),
                this.WhenValid()
            );
        }


        public ICommand TestCommand { get; }


        [Reactive]
        [Required(AllowEmptyStrings = false)]
        [Phone]
        public string Phone { get; set; }


        [Reactive]
        [EmailAddress]
        public string Email { get; set; }


        [Reactive]
        [MinLength(3, ErrorMessageResourceName = "")]
        [MaxLength(5, ErrorMessageResourceName = "")]
        public string LengthTest { get; set; }
    }
}
