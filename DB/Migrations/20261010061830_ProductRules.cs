using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DB.Migrations
{
    /// <inheritdoc />
    public partial class ProductRules : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddUniqueConstraint(
                name: "AK_Products_Name",
                table: "Products",
                column: "Name");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Deliveries_Number",
                table: "Deliveries",
                column: "Number");

            migrationBuilder.AddCheckConstraint(
                name: "ST_Count",
                table: "Stocks",
                sql: "\"Count\" > 0");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "ST_Count",
                table: "Stocks");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Products_Name",
                table: "Products");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Deliveries_Number",
                table: "Deliveries");
        }
    }
}
