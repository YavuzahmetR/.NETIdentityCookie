using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IdentityAppCookie.Web.Models
{
    public class AppDbContext : IdentityDbContext<UserApp,RoleApp,string>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
        {
            
        }
    }
}
