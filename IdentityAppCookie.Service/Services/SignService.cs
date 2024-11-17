using IdentityAppCookie.Core.ViewModels;
using IdentityAppCookie.Repository.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace IdentityAppCookie.Service.Services
{
    public class SignService(UserManager<UserApp> userManager, SignInManager<UserApp> signInManager,
        IEmailService emailService) : ISignService
    {


        public async Task<(bool, IEnumerable<IdentityError>?)> SignUpServiceAsync(string userName, string email, string phone, string passwordConfirm)
        {
            var identityResult = await userManager.CreateAsync(new UserApp()
            {
                UserName = userName,
                Email = email,
                PhoneNumber = phone
            }, passwordConfirm);


            if (!identityResult.Succeeded)
            {
                return (false, identityResult.Errors);
            }

            var exchangeExpireClaim = new Claim("ExchangeExpireDate", DateTime.Now.AddDays(10).ToString());

            var user = await userManager.FindByNameAsync(userName);

            var claimResult = await userManager.AddClaimAsync(user!, exchangeExpireClaim);

            if (!claimResult.Succeeded)
            {
                return (false, claimResult.Errors);
            }

            return (true, null);
        }

        public async Task<(bool, IEnumerable<IdentityError>?)> SignInServiceAsync(string email, string password, bool rememberMe)
        {
            var hasUser = await userManager.FindByEmailAsync(email);


            if (hasUser == null)
            {
                return (false, new List<IdentityError> { new IdentityError { Description = "Email or Password is wrong." } });
            }

            var signInResult = await signInManager.PasswordSignInAsync(hasUser, password,
                rememberMe, true);


            if (signInResult.IsLockedOut)
            {
                return (false, new List<IdentityError> { new IdentityError { Description = "You Can't Log In For 1 Minute!" } });
            }

            if (!signInResult.Succeeded)
            {
                var failedAttempts = await userManager.GetAccessFailedCountAsync(hasUser);
                return (false, new List<IdentityError> { new IdentityError { Description = $"Email or Password is wrong. Failed Access Attempts: {failedAttempts}" } });
            }

            if (hasUser.BirthDate.HasValue)
            {
                await signInManager.SignInWithClaimsAsync(hasUser, rememberMe,
                new[] { new Claim("Birthdate", hasUser.BirthDate.Value.ToString()) });
            }
            return (true,null);
        }

        public async Task<(UserApp,string)> ForgotPasswordServiceAsync(string email)
        {
            var hasUser = await userManager.FindByEmailAsync(email);

            if (hasUser == null)
            {
                throw new InvalidOperationException("There is no user with the specified email.");
            }

            string passwordResetToken = await userManager.GeneratePasswordResetTokenAsync(hasUser!);

            return (hasUser,passwordResetToken);
        }
        public async Task ResetPasswordEmailServiceAsync(string passwordResetLink,string email)
        {
            await emailService.SendResetPasswordEmailAsync(passwordResetLink,email);
        }

        public async Task<(bool, IEnumerable<IdentityError>?)> ResetPasswordServiceAsync(object userId, object token, string password)
        {
            var hasUser = await userManager.FindByIdAsync(userId.ToString()!);
            if (hasUser == null)
            {
                return (false, new List<IdentityError> { new IdentityError {Description = "User Not Found"} });
            }
            var result = await userManager.ResetPasswordAsync(hasUser, token!.ToString()!, password);

            if (!result.Succeeded)
            {
                return (false, result.Errors);
            }
            return (true, null);
        }
        public ChallengeResult ExternalLoginFacebookAuthenticationService(string redirectUrl, string authenticationSchema)
        {
            var properties = signInManager.ConfigureExternalAuthenticationProperties("Facebook", redirectUrl);
            return new ChallengeResult(authenticationSchema,properties);
        }

        public ChallengeResult ExternalLoginGoogleAuthenticationService(string redirectUrl, string authenticationSchema)
        {
            var properties = signInManager.ConfigureExternalAuthenticationProperties("Google", redirectUrl);
            return new ChallengeResult(authenticationSchema, properties);
        }

        public async Task<ExternalLoginInfo> ExternalLoginInfoService()
        {
            ExternalLoginInfo info = (await signInManager.GetExternalLoginInfoAsync())!;
            return info;
        }

        public async Task<SignInResult> SignInResultService(ExternalLoginInfo info)
        {
           SignInResult signInResult = await signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, true);
            return signInResult;
        }

        public async Task<(UserApp?,IdentityResult?,IEnumerable<IdentityError>?)> ExternalUserAppService(ExternalLoginInfo info)
        {
            UserApp userApp = new UserApp();

            userApp.Email = (info.Principal.FindFirst(ClaimTypes.Email))!.Value;

            string ExternalUserId = (info.Principal.FindFirst(ClaimTypes.NameIdentifier))!.Value;

            if (info.Principal.HasClaim(x => x.Type == ClaimTypes.Name))
            {
                string userName = (info.Principal.FindFirst(ClaimTypes.Name))!.Value;

                userName = userName.Replace(' ', '-').ToLower() + ExternalUserId.Substring(0, 5).ToString();

                userApp.UserName = userName;
            }
            else
            {
                userApp.UserName = (info.Principal.FindFirst(ClaimTypes.Email))!.Value;
            }

            IdentityResult createResult = await userManager.CreateAsync(userApp);

            if (!createResult.Succeeded)
            {
                return (userApp, createResult, createResult.Errors);
            }

            return (userApp,createResult,null);

            
        }

        public async Task<(bool,IEnumerable<IdentityError>)?> AddExternalLoginServiceAsync(UserApp userApp, ExternalLoginInfo info)
        {
            IdentityResult loginResult = await userManager.AddLoginAsync(userApp, info);
            if (loginResult.Succeeded)
            {
                await signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, true);
                return (true, null)!;
            }
            else
            {
                return (false, loginResult.Errors);
            }
        }
    }
}
