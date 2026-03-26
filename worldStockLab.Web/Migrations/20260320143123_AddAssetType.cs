using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace worldStockLab.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddAssetType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AssetType",
                table: "StockPrices",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AssetType",
                table: "StockPrices");
        }
    }
}
