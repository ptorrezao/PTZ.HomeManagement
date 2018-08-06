using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace PTZ.HomeManagement.Migrations
{
    public partial class AddIBANtoBankAccount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Abbreviation",
                table: "BankAccounts");

            migrationBuilder.AddColumn<int>(
                name: "AccountType",
                table: "BankAccounts",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Bank",
                table: "BankAccounts",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "IBAN",
                table: "BankAccounts",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsVisible",
                table: "BankAccounts",
                nullable: false,
                defaultValue: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccountType",
                table: "BankAccounts");

            migrationBuilder.DropColumn(
                name: "Bank",
                table: "BankAccounts");

            migrationBuilder.DropColumn(
                name: "IBAN",
                table: "BankAccounts");

            migrationBuilder.DropColumn(
                name: "IsVisible",
                table: "BankAccounts");

            migrationBuilder.AddColumn<string>(
                name: "Abbreviation",
                table: "BankAccounts",
                maxLength: 3,
                nullable: false,
                defaultValue: "");
        }
    }
}
