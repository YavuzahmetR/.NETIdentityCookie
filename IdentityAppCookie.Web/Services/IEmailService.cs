namespace IdentityAppCookie.Web.Services
{
    public interface IEmailService
    {

        Task SendResetPasswordEmailAsync(string resetPasswordEmailLink, string ToEmail);

    }
}
