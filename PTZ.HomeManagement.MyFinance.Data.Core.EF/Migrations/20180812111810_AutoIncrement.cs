using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace PTZ.HomeManagement.MyFinance.Data.EF.Migrations
{
    public partial class AutoIncrement : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateSequence(
                name: "BankAccount");

            migrationBuilder.CreateSequence(
                name: "BankAccountMovement");

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                table: "BankAccounts",
                nullable: false,
                defaultValueSql: "nextval('\"BankAccount\"')",
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<long>(
                name: "BankAccountId",
                table: "BankAccountMovements",
                nullable: true,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                table: "BankAccountMovements",
                nullable: false,
                defaultValueSql: "nextval('\"BankAccountMovement\"')",
                oldClrType: typeof(int));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropSequence(
                name: "BankAccount");

            migrationBuilder.DropSequence(
                name: "BankAccountMovement");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "BankAccounts",
                nullable: false,
                oldClrType: typeof(long),
                oldDefaultValueSql: "nextval('\"BankAccount\"')");

            migrationBuilder.AlterColumn<int>(
                name: "BankAccountId",
                table: "BankAccountMovements",
                nullable: true,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "BankAccountMovements",
                nullable: false,
                oldClrType: typeof(long),
                oldDefaultValueSql: "nextval('\"BankAccountMovement\"')");
        }
    }
}
