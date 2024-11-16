using System.ComponentModel.DataAnnotations;

namespace IdentityAppCookie.Core.ViewModels
{
    public class ForgotPasswordViewModel
    {
        [EmailAddress(ErrorMessage = "Email format is wrong.")]
        [Required(ErrorMessage = ("Email field cannot be left blank."))]
        [Display(Name = "Enter Email : ")]
        public string Email { get; set; } = default!;
    }
}
