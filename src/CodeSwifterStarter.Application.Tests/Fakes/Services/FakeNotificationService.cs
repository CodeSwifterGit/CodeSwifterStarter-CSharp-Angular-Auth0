using System.Threading;
using System.Threading.Tasks;
using CodeSwifterStarter.Application.Interfaces;
using CodeSwifterStarter.Application.Models;

namespace CodeSwifterStarter.Application.Tests.Fakes.Services
{
    public class FakeNotificationService : INotificationService
    {
        public async Task SendAsync(string title, string message, CancellationToken cancellationToken)
        {
        }
    }
}