using Microsoft.AspNetCore.Identity;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

namespace IdentityAppCookie.Web.Localizations
{
    public class LocalizationIdentityErrorDescriber : IdentityErrorDescriber
    {
        public override IdentityError DuplicateEmail(string email)
        {
            return new() { Code = "DuplicateEmail", Description = $"{email} is already taken." };
        }

        public override IdentityError PasswordTooShort(int length)
        {
            return new() { Code = "PasswordTooShort", Description = $"Password cant be {length} characters long. " +
                $"Try at least 6 characters long." };
        }

        public override IdentityError DuplicateUserName(string userName)
        {
            return new() { Code = "DuplicateUserName", Description = $"{userName} is already taken." };
        }

    }
}
