using IdentityAppCookie.Web.Extensions;
using IdentityAppCookie.Web.Models;
using IdentityAppCookie.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.FileProviders;

namespace IdentityAppCookie.Web.Controllers
{
    [Authorize]
    public class UserController(SignInManager<UserApp> signInManager, UserManager<UserApp> userManager,
        IFileProvider fileProvider) : Controller
    {

        public async Task<IActionResult> Index()
        {
            var currentUser = (await userManager.FindByNameAsync(User.Identity!.Name!))!;

            var userViewModel = new UserViewModel { Email = currentUser.Email, 
                PhoneNumber = currentUser.PhoneNumber, 
                UserName = currentUser.UserName, PictureUrl = currentUser.Picture };

            return View(userViewModel);
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
            var currentUser = await userManager.FindByNameAsync(User.Identity!.Name!);

            var checkOldPassword = await userManager.CheckPasswordAsync(currentUser!, passwordChangeViewModel.OldPassword);

            if(!checkOldPassword)
            {
                ModelState.AddModelError(string.Empty, "Your Current Password Is Wrong.");
                return View();
            }

            var resultChangePassword = await userManager.ChangePasswordAsync(currentUser!,
                passwordChangeViewModel.OldPassword, passwordChangeViewModel.NewPassword);

            if(!resultChangePassword.Succeeded)
            {
                ModelState.AddModelErrorExtension(resultChangePassword.Errors);
            }
            await userManager.UpdateSecurityStampAsync(currentUser!);
            await signInManager.SignOutAsync();
            await signInManager.PasswordSignInAsync(currentUser!,passwordChangeViewModel.NewPassword,true,false);


            TempData["SuccessMessage"] = "Password has been changed successfully.";


            return View();
        }

        public async Task<IActionResult> EditUser()
        {
            ViewBag.GenderList = Enum.GetValues(typeof(Gender))
                             .Cast<Gender>()
                             .Select(g => new SelectListItem
                             {
                                 Text = g.ToString(),
                                 Value = ((byte)g).ToString()
                             }).ToList();

            var currentUser = await userManager.FindByNameAsync(User.Identity!.Name!);

            var userEditViewModel = new UserEditViewModel
            {
                UserName = currentUser!.UserName!,
                Email = currentUser.Email!,
                Phone = currentUser.PhoneNumber!,
                City = currentUser.City,
                BirthDate = currentUser.BirthDate,
                Gender = currentUser.Gender
            };

            return View(userEditViewModel);
        }
        [HttpPost]
        public async Task<IActionResult> EditUser(UserEditViewModel userEditViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var currentUser = await userManager.FindByNameAsync (User.Identity!.Name!);

            currentUser!.UserName = userEditViewModel.UserName;
            currentUser.Email = userEditViewModel.Email;
            currentUser.PhoneNumber = userEditViewModel.Phone;
            currentUser.City = userEditViewModel.City;
            currentUser.Gender = userEditViewModel.Gender;
            currentUser.BirthDate = userEditViewModel.BirthDate;

            if(userEditViewModel.Picture != null && userEditViewModel.Picture.Length>0)
            {
                var wwwrootFolder = fileProvider.GetDirectoryContents("wwwroot");

                var randomFileName = $"{Guid.NewGuid().ToString()}{Path.GetExtension(userEditViewModel.Picture.FileName)}"; //guidValue.jpg - guidValue.png

                var newPicturePath = Path.Combine(wwwrootFolder.First(x => x.Name == "userpicture").PhysicalPath!, randomFileName); //combined paths

                using var stream = new FileStream(newPicturePath, FileMode.Create);

                await userEditViewModel.Picture.CopyToAsync(stream);

                currentUser.Picture = randomFileName;
            }

            var updateResult = await userManager.UpdateAsync(currentUser);

            if (!updateResult.Succeeded)
            {
                ModelState.AddModelErrorExtension(updateResult.Errors);
                return View();
            }

            await userManager.UpdateSecurityStampAsync(currentUser);
            await signInManager.SignOutAsync();
            await signInManager.SignInAsync(currentUser,true);

            TempData["SuccessMessage"] = "Membership information has been updated successfully.";

            var request = new UserEditViewModel
            {
                UserName = currentUser!.UserName!,
                Email = currentUser.Email!,
                Phone = currentUser.PhoneNumber!,
                City = currentUser.City,
                BirthDate = currentUser.BirthDate,
                Gender = currentUser.Gender
            };

            return View(request);
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
            await signInManager.SignOutAsync();

            //return RedirectToAction("Index","Home");
        }
    }
}
