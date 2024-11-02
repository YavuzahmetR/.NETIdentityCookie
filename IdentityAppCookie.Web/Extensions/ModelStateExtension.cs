using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace IdentityAppCookie.Web.Extensions
{
    public static class ModelStateExtension
    {

        public static void AddModelErrorExtension(this ModelStateDictionary modelStateDictionary, List<string> errors)
        {

            errors.ForEach(error =>
            {
                modelStateDictionary.AddModelError(string.Empty, error);
            });


        }

        public static void AddModelErrorExtension(this ModelStateDictionary modelStateDictionary, IEnumerable<IdentityError> errors)
        {

            errors.ToList().ForEach(error =>
            {
                modelStateDictionary.AddModelError(string.Empty, error.Description);
            });


        }
    }
}
