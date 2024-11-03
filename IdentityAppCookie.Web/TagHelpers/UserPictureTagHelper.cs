using Microsoft.AspNetCore.Razor.TagHelpers;

namespace IdentityAppCookie.Web.TagHelpers
{
    public class UserPictureTagHelper : TagHelper
    {
        public string? PictureUrl { get; set; }
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "img";

            if (String.IsNullOrEmpty(PictureUrl))
            {
                output.Attributes.SetAttribute("src", "/userpicture/download.jpg");
            }
            else
            {
                output.Attributes.SetAttribute("src", $"/userpicture/{PictureUrl}");
            }
        }
    }
}
