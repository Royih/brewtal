using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Brewtal.Migrations
{
    public partial class Addingnotefieldsonbrew : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "Brews",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ShoppingList",
                table: "Brews",
                nullable: true);

            migrationBuilder.Sql("UPDATE BrewStepTemplates SET Target2TempFrom = null WHERE [Name] = 'Warmup' ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Notes",
                table: "Brews");

            migrationBuilder.DropColumn(
                name: "ShoppingList",
                table: "Brews");

            migrationBuilder.Sql("UPDATE BrewStepTemplates SET Target2TempFrom = 'spargeTemp' WHERE [Name] = 'Warmup' ");
        }
    }
}
