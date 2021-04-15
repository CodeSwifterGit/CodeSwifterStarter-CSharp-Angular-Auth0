using CodeSwifterStarter.Persistence.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace CodeSwifterStarter.Persistence
{
    public class CodeSwifterStarterDbContextFactory : DesignTimeDbContextFactoryBase<CodeSwifterStarterDbContext>
    {
        protected override CodeSwifterStarterDbContext CreateNewInstance(
            DbContextOptions<CodeSwifterStarterDbContext> options)
        {
            return new(options);
        }
    }
}