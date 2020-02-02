using MailKit.Net.Smtp;

using Microsoft.Extensions.Configuration;

using MimeKit;
using MimeKit.Text;

using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace MineLib.Server.Heartbeat.Services
{
    public class MailKitOptions
    {
        [Required]
        public string Username { get; set; } = default!;
        [Required]
        public string Password { get; set; } = default!;

        [Required]
        public string Host { get; set; } = default!;
        [Range(1, ushort.MaxValue)]
        public int Port { get; set; }
        [Required]
        public bool UseSSL { get; set; }
    }

    // This class is used by the application to send Email and SMS
    // when you turn on two-factor authentication in ASP.NET Identity.
    // For more details see this link http://go.microsoft.com/fwlink/?LinkID=532713
    public sealed class AuthMessageSender : IEmailSender
    {
        private readonly MailKitOptions _options;

        public AuthMessageSender(IConfiguration configuration)
        {
            _options = new MailKitOptions();
            configuration.GetSection("MailKit").Bind(_options);
        }

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress("Администрация сайта", "minelib@aragas.org"));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(TextFormat.Html) { Text = message };

            using var client = new SmtpClient();
            await client.ConnectAsync(_options.Host, _options.Port, _options.UseSSL);
            await client.AuthenticateAsync(_options.Username, _options.Password);
            await client.SendAsync(emailMessage);

            await client.DisconnectAsync(true);
        }
    }
}