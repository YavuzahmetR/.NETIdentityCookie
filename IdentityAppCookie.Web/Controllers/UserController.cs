using IdentityAppCookie.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityAppCookie.Web.Controllers
{
    [Authorize]
    public class UserController(SignInManager<UserApp> signInManager) : Controller
    {

        public IActionResult Index()
        {
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
