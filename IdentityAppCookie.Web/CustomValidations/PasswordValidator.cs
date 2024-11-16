using IdentityAppCookie.Repository.Models;
using Microsoft.AspNetCore.Identity;

namespace IdentityAppCookie.Web.CustomValidations
{
    public class PasswordValidator : IPasswordValidator<UserApp>
    {
        public Task<IdentityResult> ValidateAsync(UserManager<UserApp> manager, UserApp user, string? password)
        {
            var errors = new List<IdentityError>();
            if (password!.ToLower().Contains(user.UserName!.ToLower()))
            {
                errors.Add(new() { Code = "PasswordCantContainUserName", Description = "Password Field Can't Contain Username." });

            }

            if (password!.StartsWith("1234"))
            {
                errors.Add(new()
                {
                    Code = "PasswordCantContainConsecutiveNumbers",
                    Description = "Password Field Can't Contain " +
                    "Consecutive Numbers."
                });
            }
            if (errors.Any())
            {
                return Task.FromResult(IdentityResult.Failed(errors.ToArray()));
            }

            return Task.FromResult(IdentityResult.Success);
        }
    }
}
