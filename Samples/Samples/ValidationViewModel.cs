using ReactiveUI;
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


        [Required(AllowEmptyStrings = false)]
        [Phone]
        public string Phone { get; set; }


        [EmailAddress]
        public string Email { get; set; }


        [MinLength(3, ErrorMessageResourceName = "")]
        [MaxLength(5, ErrorMessageResourceName = "")]
        public string LengthTest { get; set; }
    }
}
