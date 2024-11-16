using IdentityAppCookie.Core.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace IdentityAppCookie.Service.Services
{
    public interface IMemberService
    {
        Task<UserViewModel> GetUserViewModelByUserNameAsync(string userName);
        Task<UserEditViewModel> GetUserEditViewModelByUserNameAsync(string userName);
        Task LogOutAsync();

        Task<bool> CheckOldPassword(string userName,string oldPassword);

        Task<(bool,IEnumerable<IdentityError>?)> ChangePasswordAsync(string userName,string oldPassword,string newPassword);

        Task<(bool,IEnumerable<IdentityError>?)> EditUser(UserEditViewModel userEditViewModel, string userName);

        List<SelectListItem> GetGenderList();

        List<UserClaimsViewModel> GetClaimsList(ClaimsPrincipal principal);
    }
}
