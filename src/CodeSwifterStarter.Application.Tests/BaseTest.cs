using CodeSwifterStarter.Application.Interfaces;
using CodeSwifterStarter.Domain;

namespace CodeSwifterStarter.Application.Tests
{
    public abstract class BaseTest
    {
        private readonly IAuthenticatedUserService _authenticatedUserService;
        private readonly ICodeSwifterStarterDbContext _context;

        protected BaseTest(ICodeSwifterStarterDbContext context, IAuthenticatedUserService authenticatedUserService)
        {
            _context = context;
            _authenticatedUserService = authenticatedUserService;
        }
    }
}