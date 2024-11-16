using IdentityAppCookie.Web.Areas.Admin.Models;
using IdentityAppCookie.Web.Extensions;
using IdentityAppCookie.Repository.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IdentityAppCookie.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class RolesController(UserManager<UserApp> userManager, RoleManager<RoleApp> roleManager) : Controller
    {
        [Authorize(Roles = "Admin,Role-Action")]
        public async Task<IActionResult> Index()
        {
            var roles = await roleManager.Roles.Select(x => new RoleViewModel()
            {
                Id = x.Id,
                Name = x.Name!
            }).ToListAsync();

            return View(roles);
        }
        [Authorize(Roles = "Role-Action")]
        public IActionResult RoleCreate()
        {
            return View();
        }
        [Authorize(Roles = "Role-Action")]
        [HttpPost]
        public async Task<IActionResult> RoleCreate(RoleCreateViewModel roleCreateViewModel)
        {
            var result = await roleManager.CreateAsync(new RoleApp() { Name = roleCreateViewModel.Name });

            if (!result.Succeeded)
            {
                ModelState.AddModelErrorExtension(result.Errors);
                return View();
            }
            TempData["SuccessMessage"] = "Role Type Created Successfully";
            return RedirectToAction(nameof(RolesController.Index));
        }
        [Authorize(Roles = "Role-Action")]
        public async Task<IActionResult> RoleUpdate(string id)
        {
            var roleToUpdate = await roleManager.FindByIdAsync(id);

            if (roleToUpdate == null)
            {
                throw new Exception("Role Couldn't Found");
            }

            var roleView = new RoleUpdateViewModel()
            {
                Id = roleToUpdate.Id,
                Name = roleToUpdate.Name!
            };

            return View(roleView);
        }
        [Authorize(Roles = "Role-Action")]
        [HttpPost]
        public async Task<IActionResult> RoleUpdate(RoleUpdateViewModel roleUpdateViewModel)
        {
            var roleToUpdate = await roleManager.FindByIdAsync(roleUpdateViewModel.Id);

            if (roleToUpdate == null)
            {
                throw new Exception("Role Couldn't Found");
            }
            roleToUpdate.Name = roleUpdateViewModel.Name;

            await roleManager.UpdateAsync(roleToUpdate);

            ViewData["SuccessMessage"] = "Role Type Updated Successfully";

            return View();
        }
        [Authorize(Roles = "Role-Action")]
        public async Task<IActionResult> RoleDelete(string id)
        {
            var roleToDelete = await roleManager.FindByIdAsync(id);

            if (roleToDelete == null)
            {
                throw new Exception("Role Couldn't Found");
            }

            var result = await roleManager.DeleteAsync(roleToDelete);

            if (!result.Succeeded)
            {
                throw new Exception(result.Errors.Select(x => x.Description).First());
            }

            TempData["SuccessMessage"] = "Role Type Deleted Successfully";

            return RedirectToAction(nameof(RolesController.Index));
        }
        [Authorize(Roles = "Admin,Role-Action")]
        public async Task<IActionResult> RoleAssign(string id)
        {
            var currentUser = await userManager.FindByIdAsync(id);
            ViewBag.userId = id;
            if (currentUser == null)
            {
                throw new Exception("User Not Found");
            }

            var roles = await roleManager.Roles.ToListAsync();

            var userRoles = await userManager.GetRolesAsync(currentUser);

            var roleViewModelList = new List<AssignRoleToUserViewModel>();


            foreach (var role in roles)
            {
                var assignRoleToUserViewModel = new AssignRoleToUserViewModel()
                {
                    Id = role.Id,
                    Name = role.Name!
                };

                if(userRoles.Contains(role.Name!))
                {
                    assignRoleToUserViewModel.Exists = true;
                }

                roleViewModelList.Add(assignRoleToUserViewModel);   
            }

            return View(roleViewModelList);
        }
        [Authorize(Roles = "Admin,Role-Action")]
        [HttpPost]
        public async Task<IActionResult> RoleAssign(string userId, List<AssignRoleToUserViewModel> assignRoleToUserViewModels)
        {
            var userToAssignRoles = await userManager.FindByIdAsync(userId);

            foreach (var roles in assignRoleToUserViewModels)
            {
                if (roles.Exists)
                {
                    await userManager.AddToRoleAsync(userToAssignRoles!, roles.Name);
                }
                else
                {
                    await userManager.RemoveFromRoleAsync(userToAssignRoles!,roles.Name);
                }

            }
            return RedirectToAction(nameof(HomeController.UserList),"Home");
        }
    }
}
