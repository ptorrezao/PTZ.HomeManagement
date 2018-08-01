using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace PTZ.HomeManagement.Migrations
{
    public partial class BankAccountMov : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BankAccountMovement_BankAccounts_BankAccountId",
                table: "BankAccountMovement");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BankAccountMovement",
                table: "BankAccountMovement");

            migrationBuilder.RenameTable(
                name: "BankAccountMovement",
                newName: "BankAccountMovements");

            migrationBuilder.RenameIndex(
                name: "IX_BankAccountMovement_BankAccountId",
                table: "BankAccountMovements",
                newName: "IX_BankAccountMovements_BankAccountId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BankAccountMovements",
                table: "BankAccountMovements",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BankAccountMovements_BankAccounts_BankAccountId",
                table: "BankAccountMovements",
                column: "BankAccountId",
                principalTable: "BankAccounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BankAccountMovements_BankAccounts_BankAccountId",
                table: "BankAccountMovements");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BankAccountMovements",
                table: "BankAccountMovements");

            migrationBuilder.RenameTable(
                name: "BankAccountMovements",
                newName: "BankAccountMovement");

            migrationBuilder.RenameIndex(
                name: "IX_BankAccountMovements_BankAccountId",
                table: "BankAccountMovement",
                newName: "IX_BankAccountMovement_BankAccountId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BankAccountMovement",
                table: "BankAccountMovement",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BankAccountMovement_BankAccounts_BankAccountId",
                table: "BankAccountMovement",
                column: "BankAccountId",
                principalTable: "BankAccounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
