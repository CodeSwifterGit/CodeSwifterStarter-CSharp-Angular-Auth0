using System.Threading;
using System.Threading.Tasks;
using CodeSwifterStarter.Application.Interfaces;
using CodeSwifterStarter.Application.Models;

namespace CodeSwifterStarter.Infrastructure.Services
{
    public class NotificationService : INotificationService
    {
        public async Task SendAsync(string title, string message, CancellationToken cancellationToken)
        {
        }
    }
}