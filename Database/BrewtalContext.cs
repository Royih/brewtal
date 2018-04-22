using Microsoft.EntityFrameworkCore;

namespace Brewtal.Database
{
    public class BrewtalContext : DbContext
    {
        public DbSet<LogSession> Sessions { get; set; }
        public DbSet<LogRecord> Records { get; set; }
        public DbSet<PidConfig> PidConfigs { get; set; }


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
