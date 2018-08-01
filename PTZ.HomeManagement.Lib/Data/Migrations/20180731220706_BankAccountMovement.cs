using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace PTZ.HomeManagement.Migrations
{
    public partial class BankAccountMovement : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BankAccountMovement",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Amount = table.Column<decimal>(nullable: false),
                    BankAccountId = table.Column<int>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    MovementDate = table.Column<DateTime>(nullable: false),
                    TotalBalanceAfterMovement = table.Column<decimal>(nullable: false),
                    ValueDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BankAccountMovement", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BankAccountMovement_BankAccounts_BankAccountId",
                        column: x => x.BankAccountId,
                        principalTable: "BankAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BankAccountMovement_BankAccountId",
                table: "BankAccountMovement",
                column: "BankAccountId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BankAccountMovement");
        }
    }
}
