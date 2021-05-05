using System.Threading;
using System.Threading.Tasks;

namespace CodeSwifterStarter.Application.Interfaces
{
    public interface INotificationService
    {
        Task SendAsync(string title, string message, CancellationToken cancellationToken);
    }
}