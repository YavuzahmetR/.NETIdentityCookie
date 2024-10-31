using IdentityAppCookie.Web.Extensions;
using IdentityAppCookie.Web.Models;
using IdentityAppCookie.Web.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace IdentityAppCookie.Web.Controllers
{
    public class HomeController(ILogger<HomeController> logger, UserManager<UserApp> userManager, SignInManager<UserApp> signInManager) : Controller
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
            returnUrl = returnUrl ?? Url.Action(nameof(Index));

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



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
