
using IdentityAppCookie.Web.OptionsModels;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace IdentityAppCookie.Web.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;

        public EmailService(IOptions<EmailSettings> options)
        {
            _emailSettings = options.Value;
        }
        public async Task SendResetPasswordEmailAsync(string resetPasswordEmailLink, string ToEmail)
        {
            var smtpClient = new SmtpClient();

            smtpClient.Host = _emailSettings.Host;
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Port = 587;
            smtpClient.Credentials = new NetworkCredential(_emailSettings.Email, _emailSettings.Password);
            smtpClient.EnableSsl = true;


            var mailMessage  = new MailMessage();
            mailMessage.From = new MailAddress(_emailSettings.Email);
            mailMessage.To.Add(ToEmail);


            mailMessage.Subject = "Localhost | Password Reset Link"; /*""*/ /**/
            mailMessage.Body = @$"
                        <h4>For resetting your password please click the link below.</h4> 
                        <p><a href='{resetPasswordEmailLink}'> Password Reset Link.</a></p>";
            mailMessage.IsBodyHtml = true;
            await smtpClient.SendMailAsync(mailMessage);
        }
    }
}
