using IdentityAppCookie.Core.Models;
using Microsoft.AspNetCore.Identity;

namespace IdentityAppCookie.Repository.Models
{
    public class UserApp : IdentityUser
    {
        public string? City { get; set; }
        public string? Picture { get; set; }
        public DateTime? BirthDate { get; set; }
        public Gender? Gender { get; set; }
    }
}
