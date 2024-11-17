using IdentityAppCookie.Web.Extensions;
using IdentityAppCookie.Repository.Models;
using IdentityAppCookie.Core.PermissonsRoot;
using IdentityAppCookie.Web.Requirements;
using IdentityAppCookie.Repository.Seeds;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using IdentityAppCookie.Service.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContextWithMigrations(builder.Configuration);

builder.Services.AddSecurityStampValidatorOptionsExtension();

builder.Services.AddEmailSettings(builder.Configuration);

builder.Services.AddEmailService();

builder.Services.AddSignService();

builder.Services.AddViolenceRequirementService();

builder.Services.AddExchangeExpireRequirementService();

builder.Services.AddMemberService();

builder.Services.AddClaimService();

builder.Services.AddIdentityExtension();

builder.Services.ConfigureApplicationCookieExtension();

builder.Services.AddSingletonFileProvider();

builder.Services.AddAuthorizationExtension();

builder.Services.AddThirdPartyAuthenticationExtension(builder.Configuration);


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
