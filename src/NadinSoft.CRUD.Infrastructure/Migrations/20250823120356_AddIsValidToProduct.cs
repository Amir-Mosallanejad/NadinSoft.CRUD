using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NadinSoft.CRUD.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddIsValidToProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsValid",
                table: "Products",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsValid",
                table: "Products");
        }
    }
}