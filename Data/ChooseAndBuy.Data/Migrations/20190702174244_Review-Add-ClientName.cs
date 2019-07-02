using Microsoft.EntityFrameworkCore.Migrations;

namespace ChooseAndBuy.Data.Migrations
{
    public partial class ReviewAddClientName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ClientFullName",
                table: "Reviews",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClientFullName",
                table: "Reviews");
        }
    }
}
