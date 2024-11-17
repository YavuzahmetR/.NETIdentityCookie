using IdentityAppCookie.Core.ViewModels;
using IdentityAppCookie.Web.Extensions;
using IdentityAppCookie.Repository.Models;
using IdentityAppCookie.Service.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;
using Newtonsoft.Json.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace IdentityAppCookie.Web.Controllers
{
    public class HomeController(ILogger<HomeController> logger,
        UserManager<UserApp> userManager,
        SignInManager<UserApp> signInManager,
        ISignService signService) : Controller
    {
      

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpViewModel signUpViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var (isSuccess, errors) = await signService.SignUpServiceAsync(signUpViewModel.UserName,
                signUpViewModel.Email, signUpViewModel.Phone,signUpViewModel.PasswordConfirm
                );

            if (!isSuccess)
            {
                ModelState.AddModelErrorExtension(errors!);
                return View();
            }
            TempData["SuccessMessage"] = "Membership registration completed successfully.";
            return RedirectToAction(nameof(SignUp));

        }


        public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(SignInViewModel signInViewModel, string? returnUrl = null)
        {

            if (!ModelState.IsValid)
            {
                return View();
            }

            returnUrl ??= Url.Action(nameof(Index));

            var (isSuccess, errors) = await signService.SignInServiceAsync(signInViewModel.Email, signInViewModel.Password, signInViewModel.RememberMe);

            if (!isSuccess)
            {
                ModelState.AddModelErrorExtension(errors!);
                return View();
            }
            return Redirect(returnUrl);
        }

        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel forgotPasswordViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            
            var (hasUser, passwordResetToken) = await signService.ForgotPasswordServiceAsync(forgotPasswordViewModel.Email);

            var passwordResetLink = Url.Action("ResetPassword", "Home", new { userId = hasUser.Id, Token = passwordResetToken },
                HttpContext.Request.Scheme);

            await signService.ResetPasswordEmailServiceAsync(passwordResetLink!,hasUser.Email!);

            TempData["SuccessMessage"] = "Password Reset Link Has Been Send To Your Email Adress";
            return RedirectToAction(nameof(ForgotPassword));


            //example link : https://localhost:7221?userId=123213&token=asfkssfdsafasd
            //nsts ahri pqbe mvf

        }



        public IActionResult ResetPassword(string userId, string token)
        {
            TempData["userId"] = userId;
            TempData["token"] = token;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel resetPasswordViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var userId = TempData["userId"];
            var token = TempData["token"];

            if (userId == null && token == null)
            {
                throw new Exception("Somethings went wrong.");
            }

            var (isSuccess, errors) = await signService.ResetPasswordServiceAsync(userId, token, resetPasswordViewModel.Password);

            if (isSuccess)
            {
                TempData["SuccessMessage"] = "Your password has been changed successfully";
            }
            else
            {
                ModelState.AddModelErrorExtension(errors!);
                return View();
            }

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        public IActionResult GoogleLogin(string returnUrl)
        {
            string redirectUrl = Url.Action("ExternalResponse", "Home", new { returnUrl });

            return signService.ExternalLoginGoogleAuthenticationService(redirectUrl, "Google");
        }

        public IActionResult FacebookLogin(string returnUrl)
        {
            string redirectUrl = Url.Action("ExternalResponse", "Home", new { returnUrl = returnUrl });

            return signService.ExternalLoginFacebookAuthenticationService(redirectUrl, "Facebook");
            
        }


        public async Task<IActionResult> ExternalResponse(string returnUrl = "/")
        {
            var info = await signService.ExternalLoginInfoService();
            if (info == null) return RedirectToAction("SignIn");


            var signInResult = await signService.SignInResultService(info);
            if (signInResult.Succeeded) return Redirect(returnUrl);


            var (userApp, createResult, result) = await signService.ExternalUserAppService(info);

            if (createResult?.Succeeded==true)
            {
                await signService.AddExternalLoginServiceAsync(userApp!, info);
                return Redirect(returnUrl);
            }

            ModelState.AddModelErrorExtension(result!);

            var errors = ModelState.Values
                .SelectMany(x => x.Errors)
                .Select(x => x.ErrorMessage)
                .ToList();

            return RedirectToAction("CustomError", errors);
        }

        public IActionResult CustomError()
        {
            return View();
        }
    }
}
