using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DB.Migrations
{
    /// <inheritdoc />
    public partial class DishPriorityUniqueIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_DishPriorities_DishId",
                table: "DishPriorities");

            migrationBuilder.CreateIndex(
                name: "IX_DishPriorities_DishId",
                table: "DishPriorities",
                column: "DishId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_DishPriorities_DishId",
                table: "DishPriorities");

            migrationBuilder.CreateIndex(
                name: "IX_DishPriorities_DishId",
                table: "DishPriorities",
                column: "DishId");
        }
    }
}
