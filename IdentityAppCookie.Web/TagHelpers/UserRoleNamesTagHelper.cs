using IdentityAppCookie.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Text;

namespace IdentityAppCookie.Web.TagHelpers
{
    public class UserRoleNamesTagHelper(UserManager<UserApp> userManager) : TagHelper
    {
        public string UserId { get; set; } = null!;

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var user = await userManager.FindByIdAsync(UserId);

            var userRoles = await userManager.GetRolesAsync(user!);

            var stringBuilder = new StringBuilder();

            userRoles.ToList().ForEach(userRoles =>
            {
                stringBuilder.Append(@$"<span class='badge bg-secondary mx-1'>{userRoles.ToLower()}</span>");
            });
            output.Content.SetHtmlContent(stringBuilder.ToString());
        }


    }
}
