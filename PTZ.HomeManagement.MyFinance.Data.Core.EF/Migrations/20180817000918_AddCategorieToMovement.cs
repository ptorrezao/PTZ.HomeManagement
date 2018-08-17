using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace PTZ.HomeManagement.MyFinance.Data.EF.Migrations
{
    public partial class AddCategorieToMovement : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CategoriesBankAccountMovements",
                columns: table => new
                {
                    BankAccountMovementId = table.Column<long>(nullable: false),
                    CategoryId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoriesBankAccountMovements", x => new { x.BankAccountMovementId, x.CategoryId });
                    table.ForeignKey(
                        name: "FK_CategoriesBankAccountMovements_BankAccountMovements_BankAccountMovementId",
                        column: x => x.BankAccountMovementId,
                        principalTable: "BankAccountMovements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CategoriesBankAccountMovements_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CategoriesBankAccountMovements_CategoryId",
                table: "CategoriesBankAccountMovements",
                column: "CategoryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CategoriesBankAccountMovements");
        }
    }
}
