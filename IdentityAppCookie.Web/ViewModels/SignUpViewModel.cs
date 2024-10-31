using System.ComponentModel.DataAnnotations;

namespace IdentityAppCookie.Web.ViewModels
{
    public class SignUpViewModel
    {
        public SignUpViewModel()
        {
            
        }
        public SignUpViewModel(string userName, string email, string phone, string password, string passwordConfirm)
        {
            UserName = userName;
            Email = email;
            Phone = phone;
            Password = password;
            PasswordConfirm = passwordConfirm;
        }


        [Required(ErrorMessage =("Username field cannot be left blank."))]
        [Display(Name ="Username : ")]
        public string UserName { get; set; }


        [EmailAddress(ErrorMessage ="Email format is wrong.")]
        [Required(ErrorMessage = ("Email field cannot be left blank."))]
        [Display(Name = "Email : ")]
        public string Email { get; set; }


        [Required(ErrorMessage = ("Phone Number field cannot be left blank."))]
        [Display(Name = "Phone Number : ")]
        public string Phone { get; set; }



        [Required(ErrorMessage = ("Password field cannot be left blank."))]
        [Display(Name = "Password : ")]
        public string Password { get; set; }


        [Compare(nameof(Password),ErrorMessage ="Passwords are not the same.")]
        [Required(ErrorMessage = ("Password Confirm field cannot be left blank."))]
        [Display(Name = "Password Confirm : ")]
        public string PasswordConfirm { get; set; }
    }
}
