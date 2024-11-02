using IdentityAppCookie.Web.CustomValidations;
using IdentityAppCookie.Web.Localizations;
using IdentityAppCookie.Web.Models;
using IdentityAppCookie.Web.OptionsModels;
using IdentityAppCookie.Web.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;

namespace IdentityAppCookie.Web.Extensions
{
    public static class ProgramCsExtension
    {
        public static void AddIdentityExtension(this IServiceCollection services)
        {
            services.Configure<DataProtectionTokenProviderOptions>(options =>
            {
                options.TokenLifespan = TimeSpan.FromMinutes(5);
            });

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
            .AddDefaultTokenProviders()
            .AddEntityFrameworkStores<AppDbContext>();
        }
        public static void AddDbContextExtension(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(connectionString);
            });
        }


        public static void ConfigureApplicationCookieExtension(this IServiceCollection services)
        {
            services.ConfigureApplicationCookie(opt =>
            {
                var cookieBuilder = new CookieBuilder();
                cookieBuilder.Name = "RabisCookie";
                opt.Cookie = cookieBuilder;
                opt.LoginPath = new PathString("/Home/SignIn"); //Redirect Users whom does not signed in yet, for pages that will need user's cookie.
                opt.LogoutPath = new PathString("/User/Logout"); //Redirect Users to given returnUrl where it has been declared with asp-route-returnurl.
                opt.ExpireTimeSpan = TimeSpan.FromDays(60);
                opt.SlidingExpiration = true;
            });
        }


        public static void AddEmailService(this IServiceCollection services)
        {
            services.AddScoped<IEmailService, EmailService>();
        }

        public static void AddEmailSettings(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));
        }

        public static void AddSecurityStampValidatorOptionsExtension(this IServiceCollection services)
        {
            services.Configure<SecurityStampValidatorOptions>(options =>
            {
                options.ValidationInterval = TimeSpan.FromMinutes(30); // Default SecurityStamp Check Time is Already 30 minutes.
            });
        }

        public static void AddSingletonFileProvider(this IServiceCollection services)
        {
            services.AddSingleton<IFileProvider>(new PhysicalFileProvider(Directory.GetCurrentDirectory())); //IdentityAppCookie.Web File
        }

















    }
}
