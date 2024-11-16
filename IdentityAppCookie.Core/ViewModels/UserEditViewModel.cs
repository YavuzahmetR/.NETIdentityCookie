using IdentityAppCookie.Core.Models;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace IdentityAppCookie.Core.ViewModels
{
    public class UserEditViewModel
    {
        [Required(ErrorMessage = ("Username field cannot be left blank."))]
        [Display(Name = "Username : ")]
        public string UserName { get; set; } = null!;


        [EmailAddress(ErrorMessage = "Email format is wrong.")]
        [Required(ErrorMessage = ("Email field cannot be left blank."))]
        [Display(Name = "Email : ")]
        public string Email { get; set; } = null!;


        [Required(ErrorMessage = ("Phone Number field cannot be left blank."))]
        [Display(Name = "Phone Number : ")]
        public string Phone { get; set; } = null!;

        [DataType(DataType.Date)]
        [Display(Name = "Birth Date : ")]
        public DateTime? BirthDate { get; set; }


        [Display(Name = "Gender : ")]
        public Gender? Gender { get; set; }


        [Display(Name = "City : ")]
        public string? City { get; set; }


        [Display(Name = "Picture : ")]
        public IFormFile? Picture { get; set; }


    }
}
