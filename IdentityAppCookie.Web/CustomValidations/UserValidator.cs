using IdentityAppCookie.Repository.Models;
using Microsoft.AspNetCore.Identity;

namespace IdentityAppCookie.Web.CustomValidations
{
    public class UserValidator : IUserValidator<UserApp>
    {
        public Task<IdentityResult> ValidateAsync(UserManager<UserApp> manager, UserApp user)
        {
            var errors = new List<IdentityError>();

            var isDigit = int.TryParse(user.UserName![0].ToString(), out _/*This value wont be assigned to another value */);


            if (isDigit)
            {
                errors.Add(new IdentityError() { Code = "UsernameCantStartWithDigit", Description = "Username Field Can't Start With" +
                    "A Digit" });
            }

            if(errors.Count>0)
            {
                return Task.FromResult(IdentityResult.Failed(errors.ToArray()));
            }
            return Task.FromResult(IdentityResult.Success);
        }
    }
}
