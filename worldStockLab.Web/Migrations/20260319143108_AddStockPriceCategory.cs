using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace worldStockLab.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddStockPriceCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "StockPrices",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "StockPrices",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "StockPrices",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Category",
                table: "StockPrices");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "StockPrices");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "StockPrices");
        }
    }
}
