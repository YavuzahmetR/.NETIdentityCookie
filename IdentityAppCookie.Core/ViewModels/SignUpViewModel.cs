using System.ComponentModel.DataAnnotations;

namespace IdentityAppCookie.Core.ViewModels
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


        [DataType(DataType.Password)]
        [Required(ErrorMessage = ("Password field cannot be left blank."))]
        [Display(Name = "Password : ")]
        [MinLength(6, ErrorMessage = "Password Has To Contain At Least 6 Characters")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare(nameof(Password),ErrorMessage ="Passwords are not the same.")]
        [Required(ErrorMessage = ("Password Confirm field cannot be left blank."))]
        [Display(Name = "Password Confirm : ")]
        public string PasswordConfirm { get; set; }
    }
}
