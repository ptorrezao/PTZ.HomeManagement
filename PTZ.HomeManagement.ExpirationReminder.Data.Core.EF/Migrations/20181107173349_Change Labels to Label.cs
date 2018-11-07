using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace PTZ.HomeManagement.ExpirationReminder.Data.Core.EF.Migrations
{
    public partial class ChangeLabelstoLabel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Labels",
                table: "ImportSettings");

            migrationBuilder.AddColumn<string>(
                name: "Label",
                table: "ImportSettings",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Label",
                table: "ImportSettings");

            migrationBuilder.AddColumn<List<string>>(
                name: "Labels",
                table: "ImportSettings",
                nullable: true);
        }
    }
}
