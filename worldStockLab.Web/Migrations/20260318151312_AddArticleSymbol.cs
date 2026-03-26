using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace worldStockLab.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddArticleSymbol : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Symbol",
                table: "Articles",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Symbol",
                table: "Articles");
        }
    }
}
