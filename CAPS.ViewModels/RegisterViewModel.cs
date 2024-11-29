using System.ComponentModel.DataAnnotations;
using static CAPS.Global.GlobalConstants;
namespace CAPS.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = UserPasswordConfirmationErrorMsg)]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

       
    }
    
}
