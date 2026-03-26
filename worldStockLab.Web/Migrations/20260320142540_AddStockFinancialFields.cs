using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace worldStockLab.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddStockFinancialFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "High52Week",
                table: "StockPrices",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Low52Week",
                table: "StockPrices",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<long>(
                name: "MarketCap",
                table: "StockPrices",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "Volume",
                table: "StockPrices",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "High52Week",
                table: "StockPrices");

            migrationBuilder.DropColumn(
                name: "Low52Week",
                table: "StockPrices");

            migrationBuilder.DropColumn(
                name: "MarketCap",
                table: "StockPrices");

            migrationBuilder.DropColumn(
                name: "Volume",
                table: "StockPrices");
        }
    }
}
