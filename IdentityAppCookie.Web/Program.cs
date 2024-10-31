using IdentityAppCookie.Web.Extensions;
using IdentityAppCookie.Web.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContextExtension(builder.Configuration.GetConnectionString("SqlServer")!);

builder.Services.AddIdentityExtension();

builder.Services.ConfigureApplicationCookie(opt =>
{
    var cookieBuilder = new CookieBuilder();
    cookieBuilder.Name = "RabisCookie";
    opt.Cookie = cookieBuilder;
    opt.LoginPath = new PathString("/Home/SignIn"); //Redirect Users whom does not signed in yet, for pages that will need user's cookie.
    opt.LogoutPath = new PathString("/User/Logout"); //Redirect Users to given returnUrl where it has been declared with asp-route-returnurl.
    opt.ExpireTimeSpan = TimeSpan.FromDays(60);
    opt.SlidingExpiration = true;
});

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
