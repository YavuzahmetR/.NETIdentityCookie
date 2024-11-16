using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IdentityAppCookie.Web.Controllers
{
    public class OrderController : Controller
    {
        [Authorize(Policy = "OrderReadPermission")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
