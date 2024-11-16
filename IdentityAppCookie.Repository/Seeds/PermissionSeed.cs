using IdentityAppCookie.Repository.Models;
using IdentityAppCookie.Core.PermissonsRoot;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace IdentityAppCookie.Repository.Seeds
{
    public class PermissionSeed
    {
        public static async Task Seed(RoleManager<RoleApp> roleManager)
        {
            var hasBasicRole = await roleManager.RoleExistsAsync("BasicRole");
            var hasAdvancedRole = await roleManager.RoleExistsAsync("AdvancedRole");
            var hasAdminRole = await roleManager.RoleExistsAsync("AdminRole");

            if (!hasBasicRole)
            {
                await roleManager.CreateAsync(new RoleApp() { Name = "BasicRole" });

                var basicRole = await roleManager.FindByNameAsync("BasicRole");

                await AddBasicRolePermisson(roleManager, basicRole!);
                
            }
            if (!hasAdvancedRole)
            {
                await roleManager.CreateAsync(new RoleApp() { Name = "AdvancedRole" });

                var basicRole = await roleManager.FindByNameAsync("AdvancedRole");

                await AddAdvancedRolePermisson(roleManager, basicRole!);

            }
            if (!hasAdminRole)
            {
                await roleManager.CreateAsync(new RoleApp() { Name = "AdminRole" });

                var basicRole = await roleManager.FindByNameAsync("AdminRole");

                await AddAdminRolePermisson(roleManager, basicRole!);
            }
        }

        public static async Task AddBasicRolePermisson(RoleManager<RoleApp> roleManager, RoleApp roleApp)
        {
            await roleManager.AddClaimAsync(roleApp!, new Claim("Permission", Permissions.Stock.Read));
            await roleManager.AddClaimAsync(roleApp!, new Claim("Permission", Permissions.Order.Read));
            await roleManager.AddClaimAsync(roleApp!, new Claim("Permission", Permissions.Catalog.Read));
        }
        public static async Task AddAdvancedRolePermisson(RoleManager<RoleApp> roleManager, RoleApp roleApp)
        {
            await roleManager.AddClaimAsync(roleApp!, new Claim("Permission", Permissions.Stock.Read));
            await roleManager.AddClaimAsync(roleApp!, new Claim("Permission", Permissions.Order.Read));
            await roleManager.AddClaimAsync(roleApp!, new Claim("Permission", Permissions.Catalog.Read));

            await roleManager.AddClaimAsync(roleApp!, new Claim("Permission", Permissions.Stock.Create));
            await roleManager.AddClaimAsync(roleApp!, new Claim("Permission", Permissions.Order.Create));
            await roleManager.AddClaimAsync(roleApp!, new Claim("Permission", Permissions.Catalog.Create));

            await roleManager.AddClaimAsync(roleApp!, new Claim("Permission", Permissions.Stock.Update));
            await roleManager.AddClaimAsync(roleApp!, new Claim("Permission", Permissions.Order.Update));
            await roleManager.AddClaimAsync(roleApp!, new Claim("Permission", Permissions.Catalog.Update));
        }
        public static async Task AddAdminRolePermisson(RoleManager<RoleApp> roleManager, RoleApp roleApp)
        {
            await roleManager.AddClaimAsync(roleApp!, new Claim("Permission", Permissions.Stock.Read));
            await roleManager.AddClaimAsync(roleApp!, new Claim("Permission", Permissions.Order.Read));
            await roleManager.AddClaimAsync(roleApp!, new Claim("Permission", Permissions.Catalog.Read));

            await roleManager.AddClaimAsync(roleApp!, new Claim("Permission", Permissions.Stock.Create));
            await roleManager.AddClaimAsync(roleApp!, new Claim("Permission", Permissions.Order.Create));
            await roleManager.AddClaimAsync(roleApp!, new Claim("Permission", Permissions.Catalog.Create));

            await roleManager.AddClaimAsync(roleApp!, new Claim("Permission", Permissions.Stock.Update));
            await roleManager.AddClaimAsync(roleApp!, new Claim("Permission", Permissions.Order.Update));
            await roleManager.AddClaimAsync(roleApp!, new Claim("Permission", Permissions.Catalog.Update));

            await roleManager.AddClaimAsync(roleApp!, new Claim("Permission", Permissions.Stock.Delete));
            await roleManager.AddClaimAsync(roleApp!, new Claim("Permission", Permissions.Order.Delete));
            await roleManager.AddClaimAsync(roleApp!, new Claim("Permission", Permissions.Catalog.Delete));
        }
    }
}