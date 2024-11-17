using IdentityAppCookie.Web.ClaimProvider;
using IdentityAppCookie.Web.CustomValidations;
using IdentityAppCookie.Web.Localizations;
using IdentityAppCookie.Core.OptionsModels;
using IdentityAppCookie.Service.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using IdentityAppCookie.Repository.Models;
using Microsoft.AspNetCore.Authorization;
using IdentityAppCookie.Web.Requirements;
using IdentityAppCookie.Core.PermissonsRoot;

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
                options.User.AllowedUserNameCharacters = "abcçdefgğhıijklmnoöpqrşstuüvwxyzABCDEFGHIJKLMNOPQRSŞTUVWXYZ1234567890_.- ";

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

        public static void AddAuthorizationExtension(this IServiceCollection services)
        {
            services.AddAuthorization(opt =>
            {
                opt.AddPolicy("IstanbulPolicy", policy =>
                {
                    // Claim Exists On Database As A Column Type Claim - AspNetUsers
                    policy.RequireClaim("city", "İstanbul");
                });

                opt.AddPolicy("ExchangeExpireDatePolicy", policy =>
                {
                    // Claim Does Not Exists On DataBase A Column Type - Its In AspNetUserClaims
                    policy.AddRequirements(new ExchangeExpireRequirement());
                });

                opt.AddPolicy("ViolencePolicy", policy =>
                {
                    // Claim Exists On Database As A Column Type Claim - AspNetUsers
                    policy.AddRequirements(new ViolenceRequirement() { ThresholdAge = 18 });
                });

                opt.AddPolicy("OrderReadAndCreate_StockRead_Permission", policy =>
                {
                    // Claim Exists On Database As A Column Type Role - AspNetRoles
                    policy.RequireClaim("Permission", Permissions.Order.Read);
                    policy.RequireClaim("Permission", Permissions.Order.Create);
                    policy.RequireClaim("Permission", Permissions.Stock.Read);
                });

                opt.AddPolicy("OrderReadPermission", policy =>
                {
                    // Claim Exists On Database As A Column Type Role - AspNetRoles
                    policy.RequireClaim("Permission", Permissions.Order.Read);
                });

                opt.AddPolicy("OrderCreatePermission", policy =>
                {
                    // Claim Exists On Database As A Column Type Role - AspNetRoles
                    policy.RequireClaim("Permission", Permissions.Order.Create);

                });

                opt.AddPolicy("StockReadPermission", policy =>
                {
                    // Claim Exists On Database As A Column Type Role - AspNetRoles
                    policy.RequireClaim("Permission", Permissions.Stock.Read);
                });
            });
        }

        public static void AddThirdPartyAuthenticationExtension(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication().AddFacebook(opt =>
            {
                opt.AppId = configuration["Authentication:Facebook:AppId"]!;
                opt.AppSecret = configuration["Authentication:Facebook:AppSecret"]!;
            }).AddGoogle(opt =>
            {
                opt.ClientId = configuration["Authentication:Google:ClientId"]!;
                opt.ClientSecret = configuration["Authentication:Google:ClientSecret"]!;
            });
        }
        
        public static IServiceCollection AddDbContextWithMigrations(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("SqlServer"), sqlOptions =>
                {
                    sqlOptions.MigrationsAssembly("IdentityAppCookie.Repository");
                });
            });

            return services;
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
                opt.AccessDeniedPath = new PathString("/User/AccessDenied");//Redirect Users to A Custom Unauthorize Error Page.
                opt.ExpireTimeSpan = TimeSpan.FromDays(60);
                opt.SlidingExpiration = true;
            });
        }


        public static void AddEmailService(this IServiceCollection services)
        {
            services.AddScoped<IEmailService, EmailService>();
        }
        public static void AddExchangeExpireRequirementService(this IServiceCollection services)
        {
            services.AddScoped<IAuthorizationHandler, ExchangeExpireRequirementHandler>();
        }
        public static void AddViolenceRequirementService(this IServiceCollection services)
        {
            services.AddScoped<IAuthorizationHandler, ViolenceRequirementHandler>();
        }

        public static void AddMemberService(this IServiceCollection services)
        {
            services.AddScoped<IMemberService, MemberService>();
        }

        public static void AddClaimService(this IServiceCollection services)
        {
            services.AddScoped<IClaimsTransformation, UserClaimProvider>();
        }

        public static void AddSignService(this IServiceCollection services)
        {
            services.AddScoped<ISignService, SignService>();
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
