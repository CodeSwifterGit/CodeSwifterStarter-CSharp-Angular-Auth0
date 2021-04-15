using System.Threading.Tasks;
using CodeSwifterStarter.Application.Models;

namespace CodeSwifterStarter.Application.Interfaces
{
    public interface INotificationService
    {
        Task SendAsync(EmailMessage message);
    }
}