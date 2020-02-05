using System.Threading.Tasks;

namespace MineLib.Server.WebSite.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}