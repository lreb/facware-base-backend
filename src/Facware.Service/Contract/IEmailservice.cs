using Facware.Domain.Settings;
using System.Threading.Tasks;

namespace Facware.Service.Contract
{
    public interface IEmailService
    {
        Task SendEmailAsync(MailRequest mailRequest);

    }
}