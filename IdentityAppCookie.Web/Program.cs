using IdentityAppCookie.Web.Extensions;
using IdentityAppCookie.Web.Models;
using IdentityAppCookie.Web.OptionsModels;
using IdentityAppCookie.Web.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContextExtension(builder.Configuration.GetConnectionString("SqlServer")!);

builder.Services.AddSecurityStampValidatorOptionsExtension();

builder.Services.AddEmailSettings(builder.Configuration);

builder.Services.AddEmailService();
 
builder.Services.AddIdentityExtension();

builder.Services.ConfigureApplicationCookieExtension();

builder.Services.AddSingletonFileProvider(); 

var app = builder.Build();

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
