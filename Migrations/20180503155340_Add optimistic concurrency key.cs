using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Brewtal.Migrations
{
    public partial class Addoptimisticconcurrencykey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OptimisticConcurrencyKey",
                table: "Brews",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OptimisticConcurrencyKey",
                table: "Brews");
        }
    }
}
