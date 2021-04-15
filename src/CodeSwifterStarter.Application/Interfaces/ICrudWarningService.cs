using System.Threading.Tasks;
using CodeSwifterStarter.Application.Models;

namespace CodeSwifterStarter.Application.Interfaces
{
    public interface ICrudWarningService
    {
        Task SendAsync(CrudWarningMessage message);
    }
}