// <auto-generated />
using Brewtal.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;

namespace Brewtal.Migrations
{
    [DbContext(typeof(BrewtalContext))]
    [Migration("20180422134817_Add pid-config")]
    partial class Addpidconfig
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.2-rtm-10011");

            modelBuilder.Entity("Brewtal.Database.LogRecord", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<double>("ActualTemp1");

                    b.Property<double>("ActualTemp2");

                    b.Property<bool>("Output1");

                    b.Property<bool>("Output2");

                    b.Property<int>("SessionId");

                    b.Property<double>("TargetTemp1");

                    b.Property<double>("TargetTemp2");

                    b.Property<DateTime>("TimeStamp");

                    b.HasKey("Id");

                    b.HasIndex("SessionId");

                    b.ToTable("Records");
                });

            modelBuilder.Entity("Brewtal.Database.LogSession", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime?>("Completed");

                    b.Property<DateTime>("Created");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Sessions");
                });

            modelBuilder.Entity("Brewtal.Database.PidConfig", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<double>("PIDKd");

                    b.Property<double>("PIDKi");

                    b.Property<double>("PIDKp");

                    b.Property<int>("PidId");

                    b.HasKey("Id");

                    b.ToTable("PidConfigs");
                });

            modelBuilder.Entity("Brewtal.Database.LogRecord", b =>
                {
                    b.HasOne("Brewtal.Database.LogSession", "Session")
                        .WithMany("LogRecords")
                        .HasForeignKey("SessionId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
