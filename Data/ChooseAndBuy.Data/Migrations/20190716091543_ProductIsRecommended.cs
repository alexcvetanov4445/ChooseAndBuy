using Microsoft.EntityFrameworkCore.Migrations;

namespace ChooseAndBuy.Data.Migrations
{
    public partial class ProductIsRecommended : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsHidden",
                table: "Products",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsHidden",
                table: "Products");
        }
    }
}
