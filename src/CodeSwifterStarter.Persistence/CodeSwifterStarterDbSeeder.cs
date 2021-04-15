using CodeSwifterStarter.Common.Models;
using CodeSwifterStarter.Domain;
using CodeSwifterStarter.Persistence.Models;
using Microsoft.Extensions.Logging;

namespace CodeSwifterStarter.Persistence
{
    public class CodeSwifterStarterDbSeeder
    {
        private readonly ICodeSwifterStarterDbContext _context;
        private readonly ILogger<CodeSwifterStarterDbSeeder> _logger;
        private readonly ServerConfiguration _serverConfiguration;

        public CodeSwifterStarterDbSeeder(ICodeSwifterStarterDbContext context,
            ServerConfiguration serverConfiguration, ILogger<CodeSwifterStarterDbSeeder> logger)
        {
            _context = context;
            _serverConfiguration = serverConfiguration;
            _logger = logger;
        }

        public virtual void Seed(SeedType seedType)
        {
            _context.Database.EnsureCreated();

            // Enter seed code here
        }
    }
}