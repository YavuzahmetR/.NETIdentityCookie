using IdentityAppCookie.Web.Extensions;
using IdentityAppCookie.Repository.Models;
using IdentityAppCookie.Core.PermissonsRoot;
using IdentityAppCookie.Web.Requirements;
using IdentityAppCookie.Repository.Seeds;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContextWithMigrations(builder.Configuration);


builder.Services.AddSecurityStampValidatorOptionsExtension();

builder.Services.AddEmailSettings(builder.Configuration);

builder.Services.AddEmailService();

builder.Services.AddMemberService();

builder.Services.AddClaimService();

builder.Services.AddAuthorization(opt =>
{
    opt.AddPolicy("IstanbulPolicy", policy =>
    {
        // Claim Exists On Database As A Column Type Claim - AspNetUsers
        policy.RequireClaim("city", "Ýstanbul"); 
    });

    opt.AddPolicy("ExchangeExpireDatePolicy", policy =>
    {
        // Claim Does Not Exists On DataBase A Column Type - Its In AspNetUserClaims
        policy.AddRequirements(new ExchangeExpireRequirement()); 
    });

    opt.AddPolicy("ViolencePolicy", policy =>
    {
        // Claim Exists On Database As A Column Type Claim - AspNetUsers
        policy.AddRequirements(new ViolenceRequirement() { ThresholdAge = 18});
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

builder.Services.AddScoped<IAuthorizationHandler, ExchangeExpireRequirementHandler>();
builder.Services.AddScoped<IAuthorizationHandler, ViolenceRequirementHandler>();

builder.Services.AddIdentityExtension();

builder.Services.ConfigureApplicationCookieExtension();

builder.Services.AddSingletonFileProvider(); 

var app = builder.Build();

using(var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<RoleApp>>();
    await PermissionSeed.Seed(roleManager);
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();


app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
