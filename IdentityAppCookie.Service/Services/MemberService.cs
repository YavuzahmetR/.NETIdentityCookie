using IdentityAppCookie.Core.Models;
using IdentityAppCookie.Core.ViewModels;
using IdentityAppCookie.Repository.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.FileProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace IdentityAppCookie.Service.Services
{
    public class MemberService(UserManager<UserApp> userManager, SignInManager<UserApp> signInManager,
        IFileProvider fileProvider ) : IMemberService
    {
        public async Task<bool> CheckOldPassword(string userName, string oldPassword)
        {
            var currentUser = await userManager.FindByNameAsync(userName);

            return await userManager.CheckPasswordAsync(currentUser!, oldPassword);
        }

        public async Task<UserViewModel> GetUserViewModelByUserNameAsync(string userName)
        {
            var currentUser = (await userManager.FindByNameAsync(userName))!;

            var userViewModel = new UserViewModel
            {
                Email = currentUser.Email,
                PhoneNumber = currentUser.PhoneNumber,
                UserName = currentUser.UserName,
                PictureUrl = currentUser.Picture
            };

            return userViewModel;

        }

        public async Task LogOutAsync()
        {
            await signInManager.SignOutAsync();
        }

        public async Task<(bool, IEnumerable<IdentityError>?)> ChangePasswordAsync(string userName, string oldPassword, string newPassword)
        {
            var currentUser = (await userManager.FindByNameAsync(userName))!;

            var resultChangePassword = await userManager.ChangePasswordAsync(currentUser!,
               oldPassword, newPassword);

            if (!resultChangePassword.Succeeded)
            {
                return (false, resultChangePassword.Errors); 
            }
            await userManager.UpdateSecurityStampAsync(currentUser!);
            await signInManager.SignOutAsync();
            await signInManager.PasswordSignInAsync(currentUser!, newPassword, true, false);
            
            return (true,null);
        }

        public async Task<UserEditViewModel> GetUserEditViewModelByUserNameAsync(string userName)
        {
            var currentUser = await userManager.FindByNameAsync(userName);

            var userEditViewModel = new UserEditViewModel
            {
                UserName = currentUser!.UserName!,
                Email = currentUser.Email!,
                Phone = currentUser.PhoneNumber!,
                City = currentUser.City,
                BirthDate = currentUser.BirthDate,
                Gender = currentUser.Gender
            };

            return userEditViewModel;
        }

        public List<SelectListItem> GetGenderList()
        {
            return Enum.GetValues(typeof(Gender))
                       .Cast<Gender>()
                       .Select(g => new SelectListItem
                       {
                           Text = g.ToString(),
                           Value = ((byte)g).ToString()
                       }).ToList();
        }

        public async Task<(bool, IEnumerable<IdentityError>?)> EditUser(UserEditViewModel userEditViewModel, string userName)
        {

            var currentUser = await userManager.FindByNameAsync(userName);

            currentUser!.UserName = userEditViewModel.UserName;
            currentUser.Email = userEditViewModel.Email;
            currentUser.PhoneNumber = userEditViewModel.Phone;
            currentUser.City = userEditViewModel.City;
            currentUser.Gender = userEditViewModel.Gender;
            currentUser.BirthDate = userEditViewModel.BirthDate;

            if (userEditViewModel.Picture != null && userEditViewModel.Picture.Length > 0)
            {
                var wwwrootFolder = fileProvider.GetDirectoryContents("wwwroot");

                var randomFileName = $"{Guid.NewGuid().ToString()}{Path.GetExtension(userEditViewModel.Picture.FileName)}"; //guidValue.jpg - guidValue.png

                var newPicturePath = Path.Combine(wwwrootFolder.First(x => x.Name == "userpicture").PhysicalPath!, randomFileName); //combined paths

                using var stream = new FileStream(newPicturePath, FileMode.Create);

                await userEditViewModel.Picture.CopyToAsync(stream);

                currentUser.Picture = randomFileName;
            }

            var updateResult = await userManager.UpdateAsync(currentUser);

            if (!updateResult.Succeeded)
            {
                return (false,updateResult.Errors);
            }

            await userManager.UpdateSecurityStampAsync(currentUser);
            await signInManager.SignOutAsync();

            if (userEditViewModel.BirthDate.HasValue)
            {
                await signInManager.SignInWithClaimsAsync(currentUser!, true, new[] {new Claim("Birthdate",
                    currentUser.BirthDate!.Value.ToString())});
            }
            else
            {
                await signInManager.SignInAsync(currentUser, true);
            }

            return (true,null);
        }

        public List<UserClaimsViewModel> GetClaimsList(ClaimsPrincipal principal)
        {
            var userClaimList = principal.Claims.Select(x => new UserClaimsViewModel()
            {
                Issuer = x.Issuer,
                Type = x.Type,
                Value = x.Value
            }).ToList();

            return userClaimList;
        }
    }
}
