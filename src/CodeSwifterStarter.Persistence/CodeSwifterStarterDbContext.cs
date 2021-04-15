using CodeSwifterStarter.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace CodeSwifterStarter.Persistence
{
    public class CodeSwifterStarterDbContext : DbContext, ICodeSwifterStarterDbContext
    {
        public CodeSwifterStarterDbContext(DbContextOptions<CodeSwifterStarterDbContext> options)
            : base(options)
        {
        }

        #region Detect Changes

        public void SetAutoDetectChanges(bool enabled)
        {
            ChangeTracker.AutoDetectChangesEnabled = enabled;
        }

        #endregion

        #region OnModelCreating

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(CodeSwifterStarterDbContext).Assembly);
        }

        #endregion

        #region Entity Definitions

        #endregion

        #region Upgrade Database

        public void UpgradeDatabase()
        {
            Database.Migrate();
        }

        public void InitialiseDesignTime()
        {
            var modelBuilder = new ModelBuilder(new ConventionSet());

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(CodeSwifterStarterDbContext).Assembly);
        }

        #endregion
    }
}