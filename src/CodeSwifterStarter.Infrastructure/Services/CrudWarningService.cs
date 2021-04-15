using System.Threading.Tasks;
using CodeSwifterStarter.Application.Interfaces;
using CodeSwifterStarter.Application.Models;
using CodeSwifterStarter.Domain;

namespace CodeSwifterStarter.Infrastructure.Services
{
    public class CrudWarningService : ICrudWarningService
    {
        private readonly ICodeSwifterStarterDbContext _ctx;
        private readonly INotificationService _notificationService;

        public CrudWarningService(INotificationService notificationService, ICodeSwifterStarterDbContext ctx)
        {
            _notificationService = notificationService;
            _ctx = ctx;
        }

        public Task SendAsync(CrudWarningMessage message)
        {
            // Here, we need to decide, who will receive warnings about CRUD updates

            return Task.FromResult(true);
        }
    }
}