using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace PTZ.HomeManagement.ExpirationReminder.Data.Core.EF.Migrations
{
    public partial class AddCategories_Relations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReminderCategoryReminder_ReminderCategories_CategoryId",
                table: "ReminderCategoryReminder");

            migrationBuilder.DropForeignKey(
                name: "FK_ReminderCategoryReminder_Reminders_ReminderId",
                table: "ReminderCategoryReminder");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ReminderCategoryReminder",
                table: "ReminderCategoryReminder");

            migrationBuilder.RenameTable(
                name: "ReminderCategoryReminder",
                newName: "CategoriesOnReminders");

            migrationBuilder.RenameIndex(
                name: "IX_ReminderCategoryReminder_CategoryId",
                table: "CategoriesOnReminders",
                newName: "IX_CategoriesOnReminders_CategoryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CategoriesOnReminders",
                table: "CategoriesOnReminders",
                columns: new[] { "ReminderId", "CategoryId" });

            migrationBuilder.AddForeignKey(
                name: "FK_CategoriesOnReminders_ReminderCategories_CategoryId",
                table: "CategoriesOnReminders",
                column: "CategoryId",
                principalTable: "ReminderCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CategoriesOnReminders_Reminders_ReminderId",
                table: "CategoriesOnReminders",
                column: "ReminderId",
                principalTable: "Reminders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CategoriesOnReminders_ReminderCategories_CategoryId",
                table: "CategoriesOnReminders");

            migrationBuilder.DropForeignKey(
                name: "FK_CategoriesOnReminders_Reminders_ReminderId",
                table: "CategoriesOnReminders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CategoriesOnReminders",
                table: "CategoriesOnReminders");

            migrationBuilder.RenameTable(
                name: "CategoriesOnReminders",
                newName: "ReminderCategoryReminder");

            migrationBuilder.RenameIndex(
                name: "IX_CategoriesOnReminders_CategoryId",
                table: "ReminderCategoryReminder",
                newName: "IX_ReminderCategoryReminder_CategoryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ReminderCategoryReminder",
                table: "ReminderCategoryReminder",
                columns: new[] { "ReminderId", "CategoryId" });

            migrationBuilder.AddForeignKey(
                name: "FK_ReminderCategoryReminder_ReminderCategories_CategoryId",
                table: "ReminderCategoryReminder",
                column: "CategoryId",
                principalTable: "ReminderCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ReminderCategoryReminder_Reminders_ReminderId",
                table: "ReminderCategoryReminder",
                column: "ReminderId",
                principalTable: "Reminders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
