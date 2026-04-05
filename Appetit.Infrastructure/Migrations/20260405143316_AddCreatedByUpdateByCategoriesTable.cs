using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Appetit.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCreatedByUpdateByCategoriesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CreatedById",
                table: "Categories",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UpdatedById",
                table: "Categories",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Categories_CreatedById",
                table: "Categories",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_UpdatedById",
                table: "Categories",
                column: "UpdatedById");

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_Users_CreatedById",
                table: "Categories",
                column: "CreatedById",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_Users_UpdatedById",
                table: "Categories",
                column: "UpdatedById",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Categories_Users_CreatedById",
                table: "Categories");

            migrationBuilder.DropForeignKey(
                name: "FK_Categories_Users_UpdatedById",
                table: "Categories");

            migrationBuilder.DropIndex(
                name: "IX_Categories_CreatedById",
                table: "Categories");

            migrationBuilder.DropIndex(
                name: "IX_Categories_UpdatedById",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "UpdatedById",
                table: "Categories");
        }
    }
}
