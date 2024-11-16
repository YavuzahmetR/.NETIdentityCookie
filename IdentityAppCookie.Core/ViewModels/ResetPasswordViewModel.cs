using System.ComponentModel.DataAnnotations;

namespace IdentityAppCookie.Core.ViewModels
{
    public class ResetPasswordViewModel
    {

        [DataType(DataType.Password)]
        [Required(ErrorMessage = ("Password field cannot be left blank."))]
        [Display(Name = " New Password : ")]
        [MinLength(6, ErrorMessage = "Password Has To Contain At Least 6 Characters")]
        public string Password { get; set; } = null!;

        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Passwords are not the same.")]
        [Required(ErrorMessage = ("Password Confirm field cannot be left blank."))]
        [Display(Name = "New Password Confirm : ")]

        public string PasswordConfirm { get; set; } = null!;

    }
}
