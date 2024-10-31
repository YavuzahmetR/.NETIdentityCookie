using System.ComponentModel.DataAnnotations;

namespace IdentityAppCookie.Web.ViewModels
{
    public class SignInViewModel
    {
        public SignInViewModel()
        {
        }

        public SignInViewModel(string email, string password)
        {
            Email = email;
            Password = password;
        }

        [EmailAddress(ErrorMessage = "Email format is wrong.")]
        [Required(ErrorMessage = ("Email field cannot be left blank."))]
        [Display(Name = "Email : ")]
        public string Email { get; set; }


        [Required(ErrorMessage = ("Password field cannot be left blank."))]
        [Display(Name = "Password : ")]
        public string Password { get; set; }

        [Display(Name = "Remember Me : ")]
        public bool RememberMe { get; set; }    

    }
}
