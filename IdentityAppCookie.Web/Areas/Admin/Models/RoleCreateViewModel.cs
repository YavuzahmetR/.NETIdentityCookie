using System.ComponentModel.DataAnnotations;

namespace IdentityAppCookie.Web.Areas.Admin.Models
{
    public class RoleCreateViewModel 
    {
        [Required(ErrorMessage = ("Role Name field cannot be left blank."))]
        [Display(Name = "Role Name : ")]
        public string Name { get; set; } = null!;
    }
}
