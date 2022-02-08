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
        [Required(AllowEmptyStrings = false, ErrorMessage = "Required Field")]
        [Phone(ErrorMessage = "Invalid Phone #")]
        public string Phone { get; set; }


        [Reactive]
        [EmailAddress(ErrorMessage = "Invalid Email")]
        public string Email { get; set; }


        [Reactive]
        [MinLength(3, ErrorMessage = "Min 3 Characters")]
        [MaxLength(5, ErrorMessage = "Max 5 Characters")]
        public string LengthTest { get; set; }
    }
}
