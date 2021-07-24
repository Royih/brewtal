using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Brewtal.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Sessions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    StartTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    TargetTemp = table.Column<double>(type: "REAL", nullable: false),
                    MinTemp = table.Column<double>(type: "REAL", nullable: false),
                    MaxTemp = table.Column<double>(type: "REAL", nullable: false),
                    TimeToReachTarget = table.Column<TimeSpan>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sessions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Runtime",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CurrentSessionId = table.Column<int>(type: "INTEGER", nullable: true),
                    Startup = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Runtime", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Runtime_Sessions_CurrentSessionId",
                        column: x => x.CurrentSessionId,
                        principalTable: "Sessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Templogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SessionId = table.Column<int>(type: "INTEGER", nullable: false),
                    TimeStamp = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ActualTemperature = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Templogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Templogs_Sessions_SessionId",
                        column: x => x.SessionId,
                        principalTable: "Sessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Runtime_CurrentSessionId",
                table: "Runtime",
                column: "CurrentSessionId");

            migrationBuilder.CreateIndex(
                name: "IX_Templogs_SessionId",
                table: "Templogs",
                column: "SessionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Runtime");

            migrationBuilder.DropTable(
                name: "Templogs");

            migrationBuilder.DropTable(
                name: "Sessions");
        }
    }
}
