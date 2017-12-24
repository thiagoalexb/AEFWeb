using System.Threading.Tasks;

namespace AEFWeb.Core.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}
