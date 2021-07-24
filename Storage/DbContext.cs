using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Brewtal2.Storage
{
    public class StorageContext : DbContext
    {
        public DbSet<Session> Sessions { get; set; }
        public DbSet<Runtime> Runtime { get; set; }
        public DbSet<Templog> Templogs { get; set; }
        public string DbPath { get; private set; }


        // The following configures EF to create a Sqlite database file in the
        // special "local" folder for your platform.
        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite($"Filename=./Data/Brewtal.db");


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // modelBuilder.Entity<Templog>()
            //     .HasOne(p => p.Session)
            //     .WithMany(b => b.Logs)
            //     .HasForeignKey(p => p.SessionId);

            // modelBuilder.Entity<Runtime>()
            // .HasOne<Session>()
            // .WithMany()
            // .HasForeignKey(p => p.CurrentSessionId);

        }


    }


    public class Runtime
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }
        public int? CurrentSessionId { get; set; }
        public Session CurrentSession { get; set; }
        public DateTime Startup { get; set; }
    }

    public class Templog
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }
        public int SessionId { get; set; }
        public DateTime TimeStamp { get; set; }
        public double ActualTemperature { get; set; }
        public Session Session { get; set; }
    }

    public class Session
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }
        public DateTime StartTime { get; set; }
        public double TargetTemp { get; set; }
        public double MinTemp { get; set; }
        public double MaxTemp { get; set; }
        public TimeSpan? TimeToReachTarget { get; set; }
        public List<Templog> Logs { get; } = new List<Templog>();


    }
}