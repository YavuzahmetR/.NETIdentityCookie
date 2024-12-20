﻿using IdentityAppCookie.Web.Areas.Admin.Models;
using IdentityAppCookie.Repository.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IdentityAppCookie.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class HomeController : Controller
    {

        private readonly UserManager<UserApp> _userManager;

        public HomeController(UserManager<UserApp> userManager)
        {
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }
        
        public async Task<IActionResult> UserList()
        {
            var users = await _userManager.Users.ToListAsync();

            var userViewModel = users.Select(x => new AdminUserViewModel
            {
                Id = x.Id,
                Name = x.UserName,
                Email = x.Email,    
            }).ToList();

            return View(userViewModel);
        }

    }
}
