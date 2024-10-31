using IdentityAppCookie.Web.CustomValidations;
using IdentityAppCookie.Web.Localizations;
using IdentityAppCookie.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace IdentityAppCookie.Web.Extensions
{
    public static class ProgramCsExtension
    {
        public static void AddIdentityExtension(this IServiceCollection services)
        {
            services.AddIdentity<UserApp, RoleApp>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890_";

                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 6;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = true;
                options.Password.RequireDigit = false;


                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(1);
                options.Lockout.MaxFailedAccessAttempts = 3;

            }).AddUserValidator<UserValidator>().
            AddPasswordValidator<PasswordValidator>().AddErrorDescriber<LocalizationIdentityErrorDescriber>()
            .AddEntityFrameworkStores<AppDbContext>();
        }
        public static void AddDbContextExtension(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(connectionString);
            });
        }























    }
}
