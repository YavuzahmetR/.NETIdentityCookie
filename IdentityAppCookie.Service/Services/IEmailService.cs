namespace IdentityAppCookie.Service.Services
{
    public interface IEmailService
    {

        Task SendResetPasswordEmailAsync(string resetPasswordEmailLink, string ToEmail);

    }
}
