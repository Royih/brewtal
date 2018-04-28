using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace brewtal.Migrations
{
    public partial class Addbrewguidedbobjects : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Brews",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    BatchNumber = table.Column<int>(nullable: false),
                    BatchSize = table.Column<int>(nullable: false),
                    BeginMash = table.Column<DateTime>(nullable: false),
                    BoilTimeInMinutes = table.Column<int>(nullable: false),
                    Initiated = table.Column<DateTime>(nullable: false),
                    MashOutTemp = table.Column<float>(nullable: false),
                    MashTemp = table.Column<float>(nullable: false),
                    MashTimeInMinutes = table.Column<int>(nullable: false),
                    MashWaterAmount = table.Column<float>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    SpargeTemp = table.Column<float>(nullable: false),
                    SpargeWaterAmount = table.Column<float>(nullable: false),
                    StrikeTemp = table.Column<float>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Brews", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BrewStepTemplates",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CompleteButtonText = table.Column<string>(nullable: true),
                    CompleteTimeAdd = table.Column<string>(nullable: true),
                    Instructions = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    ShowTimer = table.Column<bool>(nullable: false),
                    Target1TempFrom = table.Column<string>(nullable: true),
                    Target2TempFrom = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BrewStepTemplates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BrewSteps",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    BrewId = table.Column<int>(nullable: false),
                    CompleteButtonText = table.Column<string>(nullable: true),
                    CompleteTime = table.Column<DateTime>(nullable: true),
                    Instructions = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Order = table.Column<int>(nullable: false),
                    ShowTimer = table.Column<bool>(nullable: false),
                    StartTime = table.Column<DateTime>(nullable: false),
                    TargetMashTemp = table.Column<float>(nullable: false),
                    TargetSpargeTemp = table.Column<float>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BrewSteps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BrewSteps_Brews_BrewId",
                        column: x => x.BrewId,
                        principalTable: "Brews",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DataCaptureDefinitions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    BrewStepTemplateId = table.Column<int>(nullable: false),
                    Label = table.Column<string>(nullable: true),
                    Optional = table.Column<bool>(nullable: false),
                    Units = table.Column<string>(nullable: true),
                    ValueType = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataCaptureDefinitions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DataCaptureDefinitions_BrewStepTemplates_BrewStepTemplateId",
                        column: x => x.BrewStepTemplateId,
                        principalTable: "BrewStepTemplates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DataCaptureFloatValues",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    BrewStepId = table.Column<int>(nullable: false),
                    Label = table.Column<string>(nullable: true),
                    Optional = table.Column<bool>(nullable: false),
                    Units = table.Column<string>(nullable: true),
                    Value = table.Column<float>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataCaptureFloatValues", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DataCaptureFloatValues_BrewSteps_BrewStepId",
                        column: x => x.BrewStepId,
                        principalTable: "BrewSteps",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DataCaptureIntValues",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    BrewStepId = table.Column<int>(nullable: false),
                    Label = table.Column<string>(nullable: true),
                    Optional = table.Column<bool>(nullable: false),
                    Units = table.Column<string>(nullable: true),
                    Value = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataCaptureIntValues", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DataCaptureIntValues_BrewSteps_BrewStepId",
                        column: x => x.BrewStepId,
                        principalTable: "BrewSteps",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DataCaptureStringValues",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    BrewStepId = table.Column<int>(nullable: false),
                    Label = table.Column<string>(nullable: true),
                    Optional = table.Column<bool>(nullable: false),
                    Units = table.Column<string>(nullable: true),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataCaptureStringValues", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DataCaptureStringValues_BrewSteps_BrewStepId",
                        column: x => x.BrewStepId,
                        principalTable: "BrewSteps",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BrewSteps_BrewId",
                table: "BrewSteps",
                column: "BrewId");

            migrationBuilder.CreateIndex(
                name: "IX_DataCaptureDefinitions_BrewStepTemplateId",
                table: "DataCaptureDefinitions",
                column: "BrewStepTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_DataCaptureFloatValues_BrewStepId",
                table: "DataCaptureFloatValues",
                column: "BrewStepId");

            migrationBuilder.CreateIndex(
                name: "IX_DataCaptureIntValues_BrewStepId",
                table: "DataCaptureIntValues",
                column: "BrewStepId");

            migrationBuilder.CreateIndex(
                name: "IX_DataCaptureStringValues_BrewStepId",
                table: "DataCaptureStringValues",
                column: "BrewStepId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DataCaptureDefinitions");

            migrationBuilder.DropTable(
                name: "DataCaptureFloatValues");

            migrationBuilder.DropTable(
                name: "DataCaptureIntValues");

            migrationBuilder.DropTable(
                name: "DataCaptureStringValues");

            migrationBuilder.DropTable(
                name: "BrewStepTemplates");

            migrationBuilder.DropTable(
                name: "BrewSteps");

            migrationBuilder.DropTable(
                name: "Brews");
        }
    }
}
