using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace worldStockLab.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddStockPriceViewCount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ViewCount",
                table: "StockPrices",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ViewCount",
                table: "StockPrices");
        }
    }
}
