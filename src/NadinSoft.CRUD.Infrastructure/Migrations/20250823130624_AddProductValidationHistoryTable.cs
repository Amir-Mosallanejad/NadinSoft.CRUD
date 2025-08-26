using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace NadinSoft.CRUD.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddProductValidationHistoryTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProductValidationHistory",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OldValue = table.Column<bool>(type: "bit", nullable: true),
                    NewValue = table.Column<bool>(type: "bit", nullable: false),
                    ModifyDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductValidationHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductValidationHistory_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductValidationHistory_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductValidationHistory_ProductId",
                table: "ProductValidationHistory",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductValidationHistory_UserId",
                table: "ProductValidationHistory",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductValidationHistory");
        }
    }
}