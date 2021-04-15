using System.Threading.Tasks;
using CodeSwifterStarter.Application.Interfaces;
using CodeSwifterStarter.Application.Models;

namespace CodeSwifterStarter.Infrastructure.Services
{
    public class NotificationService : INotificationService
    {
        public Task SendAsync(EmailMessage message)
        {
            return Task.CompletedTask;
        }
    }
}