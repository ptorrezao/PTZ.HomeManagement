using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace PTZ.HomeManagement.ExpirationReminder.Data.Core.EF.Migrations
{
    public partial class AddCategories_AddNotesToReminder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateSequence(
                name: "ReminderCategory");

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "Reminders",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "NotifyInPeriodAmout",
                table: "Reminders",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<int>(
                name: "NotifyInPeriodType",
                table: "Reminders",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "NotifyType",
                table: "Reminders",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ReminderCategories",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false, defaultValueSql: "nextval('\"ReminderCategory\"')"),
                    ApplicationUserId = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReminderCategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReminderCategories_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ReminderCategoryReminder",
                columns: table => new
                {
                    ReminderId = table.Column<long>(nullable: false),
                    CategoryId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReminderCategoryReminder", x => new { x.ReminderId, x.CategoryId });
                    table.ForeignKey(
                        name: "FK_ReminderCategoryReminder_ReminderCategories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "ReminderCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReminderCategoryReminder_Reminders_ReminderId",
                        column: x => x.ReminderId,
                        principalTable: "Reminders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReminderCategories_ApplicationUserId",
                table: "ReminderCategories",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ReminderCategoryReminder_CategoryId",
                table: "ReminderCategoryReminder",
                column: "CategoryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReminderCategoryReminder");

            migrationBuilder.DropTable(
                name: "ReminderCategories");

            migrationBuilder.DropSequence(
                name: "ReminderCategory");

            migrationBuilder.DropColumn(
                name: "Notes",
                table: "Reminders");

            migrationBuilder.DropColumn(
                name: "NotifyInPeriodAmout",
                table: "Reminders");

            migrationBuilder.DropColumn(
                name: "NotifyInPeriodType",
                table: "Reminders");

            migrationBuilder.DropColumn(
                name: "NotifyType",
                table: "Reminders");
        }
    }
}
