using IdentityAppCookie.Core.Models;
using IdentityAppCookie.Core.ViewModels;
using IdentityAppCookie.Web.Extensions;
using IdentityAppCookie.Repository.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.FileProviders;
using System.Security.Claims;
using IdentityAppCookie.Service.Services;

namespace IdentityAppCookie.Web.Controllers
{
    [Authorize]
    public class UserController(SignInManager<UserApp> signInManager, UserManager<UserApp> userManager,
        IFileProvider fileProvider, IMemberService memberService) : Controller
    {
        private string userName => User.Identity!.Name!;
        public async Task<IActionResult> Index() 
        {
            return View(await memberService.GetUserViewModelByUserNameAsync(userName));
        }

        [HttpGet]
        public IActionResult Claims()
        {
            return View(memberService.GetClaimsList(User));
        }
        [Authorize(Policy = "IstanbulPolicy")]
        [HttpGet]
        public IActionResult IstanbulPolicy()
        {
            return View();
        }

        [Authorize(Policy = "ExchangeExpireDatePolicy")]
        [HttpGet]
        public IActionResult ExchangeExpireDatePolicy()
        {
            return View();
        }

        [Authorize(Policy = "ViolencePolicy")]
        [HttpGet]
        public IActionResult ViolencePolicy()
        {
            return View();
        }
        public IActionResult PasswordChange()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> PasswordChange(PasswordChangeViewModel passwordChangeViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }   
               
            if(!await memberService.CheckOldPassword(userName, passwordChangeViewModel.OldPassword))
            {
                ModelState.AddModelError(string.Empty, "Your Current Password Is Wrong.");
                return View();
            }

            var (isSuccess,errors) = await memberService.ChangePasswordAsync(userName, passwordChangeViewModel.OldPassword,
                passwordChangeViewModel.NewPassword);

            if(!isSuccess)
            {
                ModelState.AddModelErrorExtension(errors!);
                return View();
            }
            
            TempData["SuccessMessage"] = "Password has been changed successfully.";


            return View();
        }

        public async Task<IActionResult> EditUser()
        {
            ViewBag.GenderList = memberService.GetGenderList();
           
            return View(await memberService.GetUserEditViewModelByUserNameAsync(userName));
        }
        [HttpPost]
        public async Task<IActionResult> EditUser(UserEditViewModel userEditViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var (isSuccess, errors) = await memberService.EditUser(userEditViewModel,userName);

            if (!isSuccess)
            {
                ModelState.AddModelErrorExtension(errors!);
                return View();
            }          

            TempData["SuccessMessage"] = "Membership information has been updated successfully.";
        

            return View(await memberService.GetUserEditViewModelByUserNameAsync(userName));
        }

        public IActionResult AccessDenied(string returnUrl)
        {
            string message = "You do not have permission to view this page. Please contact your administrator for access.";

            ViewBag.Message = message;
            
            return View();
        }

        //First way. Second way is from cookieBuilder.LogoutPath
        public async Task/*<IActionResult>*/ Logout()
        {
            await memberService.LogOutAsync();

            //return RedirectToAction("Index","Home");
        }
    }
}
