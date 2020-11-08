using Fakelaki.Api.Lib.Models;
using System.Threading.Tasks;

namespace Fakelaki.Api.Lib.Services.Interfaces
{
    public interface IMailService
    {
        Task SendEmailAsync(MailRequest mailRequest);
    }
}
