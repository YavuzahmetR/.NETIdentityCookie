using IdentityAppCookie.Repository.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace IdentityAppCookie.Service.Services
{
    public interface ISignService
    {
        Task<(bool, IEnumerable<IdentityError>?)> SignUpServiceAsync(string userName, string email, string phone, string passwordConfirm);

        Task<(bool, IEnumerable<IdentityError>?)> SignInServiceAsync(string email, string password, bool rememberMe);

        Task<(UserApp,string)> ForgotPasswordServiceAsync(string email);

        Task ResetPasswordEmailServiceAsync(string passwordResetLink, string email);

        Task<(bool, IEnumerable<IdentityError>?)> ResetPasswordServiceAsync(object userId, object token, string password);

        ChallengeResult ExternalLoginFacebookAuthenticationService(string redirectUrl, string authenticationSchema);

        ChallengeResult ExternalLoginGoogleAuthenticationService(string redirectUrl, string authenticationSchema);

        Task<ExternalLoginInfo> ExternalLoginInfoService();

        Task<SignInResult> SignInResultService(ExternalLoginInfo info);

        Task<(UserApp?, IdentityResult?, IEnumerable<IdentityError>?)> ExternalUserAppService(ExternalLoginInfo info);

        Task<(bool, IEnumerable<IdentityError>)?> AddExternalLoginServiceAsync(UserApp userApp, ExternalLoginInfo info);
    }
}
