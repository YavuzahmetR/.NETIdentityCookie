namespace IdentityAppCookie.Web.Areas.Admin.Models
{
    public class AssignRoleToUserViewModel
    {
        public string Id { get; set; } = null!;
        public string Name { get; set; } = null!;
        public bool Exists { get; set; }
    }
}
