using IdentityAppCookie.Web.Extensions;
using IdentityAppCookie.Web.Models;
using IdentityAppCookie.Web.Services;
using IdentityAppCookie.Web.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace IdentityAppCookie.Web.Controllers
{
    public class HomeController(ILogger<HomeController> logger, UserManager<UserApp> userManager, SignInManager<UserApp> signInManager, IEmailService emailService) : Controller
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

            var identityResult = await userManager.CreateAsync(new()
            {
                UserName = signUpViewModel.UserName,
                Email = signUpViewModel.Email,
                PhoneNumber = signUpViewModel.Phone
            }, signUpViewModel.PasswordConfirm);


            if (identityResult.Succeeded)
            {
                TempData["SuccessMessage"] = "Membership registration completed successfully.";
                return RedirectToAction(nameof(HomeController.SignUp));
            }

            ModelState.AddModelErrorExtension(identityResult.Errors.Select(x => x.Description).ToList());

            return View();

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

            var hasUser = await userManager.FindByEmailAsync(signInViewModel.Email);

            if (hasUser == null)
            {
                ModelState.AddModelError(string.Empty, "Email or Password is wrong."); //User does not exists.
                return View();
            }

            var signInResult = await signInManager.PasswordSignInAsync(hasUser, signInViewModel.Password,
                signInViewModel.RememberMe, true);

            if (signInResult.Succeeded)
            {
                return Redirect(returnUrl);
            }

            if (signInResult.IsLockedOut)
            {
                ModelState.AddModelError(string.Empty, "You Can't Log In For 1 Minute!");
                return View();
            }


            ModelState.AddModelErrorExtension(new List<string>() {$"Email or Password is wrong.", //User exists but password wrong.
               $"Failed Accession Attemps : {await userManager.GetAccessFailedCountAsync(hasUser)}" });


            return View();
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
            var hasUser = await userManager.FindByEmailAsync(forgotPasswordViewModel.Email);

            if (hasUser == null)
            {
                ModelState.AddModelError(string.Empty, "There is no user has this email, Try Again.");
                return View();
            }
            string passwordResetToken = await userManager.GeneratePasswordResetTokenAsync(hasUser!);

            var passwordResetLink = Url.Action("ResetPassword", "Home", new { userId = hasUser.Id , Token = passwordResetToken },
                HttpContext.Request.Scheme);

            //example link : https://localhost:7221?userId=123213&token=asfkssfdsafasd

            await emailService.SendResetPasswordEmailAsync(passwordResetLink!,hasUser.Email!);

            TempData["SuccessMessage"] = "Password Reset Link Has Been Send To Your Email Adress";
            return RedirectToAction(nameof(ForgotPassword));



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
            var userId = TempData["userId"];
            var token = TempData["token"];

            if(userId == null && token == null)
            {
                throw new Exception("Somethings went wrong.");
            }

            var hasUser = await userManager.FindByIdAsync(userId!.ToString()!);

            if(hasUser == null)
            {
                ModelState.AddModelError(string.Empty, "User Not Found.");
                return View();
            }

            var result = await userManager.ResetPasswordAsync(hasUser,token!.ToString()!, resetPasswordViewModel.Password);

            if (result.Succeeded)
            {
                TempData["SuccessMessage"] = "Your password has been changed successfully";
            }
            else
            {
                ModelState.AddModelErrorExtension(result.Errors.Select(x => x.Description).ToList());
                return View();
            }

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
