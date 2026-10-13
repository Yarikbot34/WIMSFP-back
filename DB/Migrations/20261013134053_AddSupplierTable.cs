using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DB.Migrations
{
    /// <inheritdoc />
    public partial class AddSupplierTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "ST_Count",
                table: "Stocks");

            migrationBuilder.DropCheckConstraint(
                name: "DI_Count",
                table: "DishIngredients");

            migrationBuilder.AddColumn<int>(
                name: "SupplierId",
                table: "Deliveries",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Suppliers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Suppliers", x => x.Id);
                });

            migrationBuilder.AddCheckConstraint(
                name: "ST_Count",
                table: "Stocks",
                sql: "\"Count\" >= 0");

            migrationBuilder.AddCheckConstraint(
                name: "DI_Count",
                table: "DishIngredients",
                sql: "\"Amount\" >= 0");

            migrationBuilder.CreateIndex(
                name: "IX_Deliveries_SupplierId",
                table: "Deliveries",
                column: "SupplierId");

            migrationBuilder.AddForeignKey(
                name: "FK_Deliveries_Suppliers_SupplierId",
                table: "Deliveries",
                column: "SupplierId",
                principalTable: "Suppliers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Deliveries_Suppliers_SupplierId",
                table: "Deliveries");

            migrationBuilder.DropTable(
                name: "Suppliers");

            migrationBuilder.DropCheckConstraint(
                name: "ST_Count",
                table: "Stocks");

            migrationBuilder.DropCheckConstraint(
                name: "DI_Count",
                table: "DishIngredients");

            migrationBuilder.DropIndex(
                name: "IX_Deliveries_SupplierId",
                table: "Deliveries");

            migrationBuilder.DropColumn(
                name: "SupplierId",
                table: "Deliveries");

            migrationBuilder.AddCheckConstraint(
                name: "ST_Count",
                table: "Stocks",
                sql: "\"Count\" > 0");

            migrationBuilder.AddCheckConstraint(
                name: "DI_Count",
                table: "DishIngredients",
                sql: "\"Amount\" > 0");
        }
    }
}
