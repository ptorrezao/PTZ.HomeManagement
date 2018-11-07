using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace PTZ.HomeManagement.ExpirationReminder.Data.Core.EF.Migrations
{
    public partial class ImportSettings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateSequence(
                name: "ImportSetting");

            migrationBuilder.CreateTable(
                name: "ImportSettings",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false, defaultValueSql: "nextval('\"ImportSetting\"')"),
                    ApplicationUserId = table.Column<string>(nullable: true),
                    ExpirationDateRexgex = table.Column<string>(nullable: true),
                    ExpirationDateRexgexTarget = table.Column<int>(nullable: false),
                    Labels = table.Column<List<string>>(nullable: true),
                    TitleFormat = table.Column<string>(nullable: true),
                    TitleRexgex = table.Column<string>(nullable: true),
                    TitleRexgexTarget = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImportSettings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ImportSettings_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ImportSettings_ApplicationUserId",
                table: "ImportSettings",
                column: "ApplicationUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ImportSettings");

            migrationBuilder.DropSequence(
                name: "ImportSetting");

        }
    }
}
