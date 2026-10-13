using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DB.Migrations
{
    /// <inheritdoc />
    public partial class PriceChecks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "PayAmount",
                table: "Orders",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<bool>(
                name: "HasDiscount",
                table: "OrderObjs",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<decimal>(
                name: "ItemPrice",
                table: "OrderObjs",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddCheckConstraint(
                name: "OR_Price",
                table: "Orders",
                sql: "\"PayAmount\" > 0");

            migrationBuilder.AddCheckConstraint(
                name: "ORB_Price",
                table: "OrderObjs",
                sql: "\"ItemPrice\" > 0");

            migrationBuilder.AddCheckConstraint(
                name: "Dish_Price",
                table: "Dishes",
                sql: "\"Price\" >= 0");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "OR_Price",
                table: "Orders");

            migrationBuilder.DropCheckConstraint(
                name: "ORB_Price",
                table: "OrderObjs");

            migrationBuilder.DropCheckConstraint(
                name: "Dish_Price",
                table: "Dishes");

            migrationBuilder.DropColumn(
                name: "PayAmount",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "HasDiscount",
                table: "OrderObjs");

            migrationBuilder.DropColumn(
                name: "ItemPrice",
                table: "OrderObjs");
        }
    }
}
