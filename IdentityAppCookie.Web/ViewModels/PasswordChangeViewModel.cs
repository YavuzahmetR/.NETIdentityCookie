using System.ComponentModel.DataAnnotations;

namespace IdentityAppCookie.Web.ViewModels
{
    public class PasswordChangeViewModel
    {
        [DataType(DataType.Password)]
        [Required(ErrorMessage = ("Password field cannot be left blank."))]
        [Display(Name = " Old Password : ")]
        [MinLength(6, ErrorMessage = "Password Has To Contain At Least 6 Characters")]
        public string OldPassword { get; set; } = null!;

        [DataType(DataType.Password)]
        [Required(ErrorMessage = ("Password field cannot be left blank."))]
        [Display(Name = " New Password : ")]
        [MinLength(6, ErrorMessage = "Password Has To Contain At Least 6 Characters")]
        public string NewPassword { get; set; } = null!;

        [DataType(DataType.Password)]
        [Compare(nameof(NewPassword), ErrorMessage = "Passwords are not the same.")]
        [Required(ErrorMessage = ("Password Confirm field cannot be left blank."))]
        [Display(Name = "New Password Confirm : ")]
        [MinLength(6, ErrorMessage = "Password Has To Contain At Least 6 Characters")]
        public string NewPasswordConfirm { get; set; } = null!;
    } 
}
