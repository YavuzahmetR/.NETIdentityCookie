using System.ComponentModel.DataAnnotations;

namespace IdentityAppCookie.Web.Areas.Admin.Models
{
    public class RoleUpdateViewModel
    {

        public string Id { get; set; } = null!;

        [Required(ErrorMessage = ("Role Name field cannot be left blank."))]
        [Display(Name = "Role Name : ")]
        public string Name { get; set; } = null!;
    }
}
