using MimeKit;
using MimeKit.Text;

using MailKit.Net.Smtp;

using System.Threading.Tasks;

namespace MineLib.Server.Heartbeat.Services
{
    // This class is used by the application to send Email and SMS
    // when you turn on two-factor authentication in ASP.NET Identity.
    // For more details see this link http://go.microsoft.com/fwlink/?LinkID=532713
    public class AuthMessageSender : IEmailSender
    {
        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress("Администрация сайта", "minelib@aragas.org"));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(TextFormat.Html) { Text = message };

            using var client = new SmtpClient();
            await client.ConnectAsync("smtp.zoho.eu", 465, true);
            await client.AuthenticateAsync("minelib@aragas.org", @"wLW?^BbDc\s_3<7W");
            await client.SendAsync(emailMessage);

            await client.DisconnectAsync(true);
        }
    }
}