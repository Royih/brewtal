using Microsoft.EntityFrameworkCore;

namespace Brewtal.Database
{
    public class BrewtalContext : DbContext
    {
        public DbSet<LogSession> Sessions { get; set; }
        public DbSet<LogRecord> Records { get; set; }
        public DbSet<PidConfig> PidConfigs { get; set; }
        public DbSet<Brew> Brews { get; set; }
        public DbSet<BrewStep> BrewSteps { get; set; }
        public DbSet<BrewStepTemplate> BrewStepTemplates { get; set; }
        public DbSet<DataCaptureDefinition> DataCaptureDefinitions { get; set; }
        public DbSet<DataCaptureFloatValue> DataCaptureFloatValues { get; set; }
        public DbSet<DataCaptureStringValue> DataCaptureStringValues { get; set; }
        public DbSet<DataCaptureIntValue> DataCaptureIntValues { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=brewtal.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LogRecord>()
                .HasOne(p => p.Session)
                .WithMany(b => b.LogRecords);


        }

    }

}
