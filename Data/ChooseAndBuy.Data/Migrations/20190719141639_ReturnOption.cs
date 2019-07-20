namespace ChooseAndBuy.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class ReturnOption : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ReturnReason",
                table: "Orders",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Subject",
                table: "ContactMessages",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "ContactMessages",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Message",
                table: "ContactMessages",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "ContactMessages",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReturnReason",
                table: "Orders");

            migrationBuilder.AlterColumn<string>(
                name: "Subject",
                table: "ContactMessages",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "ContactMessages",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "Message",
                table: "ContactMessages",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "ContactMessages",
                nullable: true,
                oldClrType: typeof(string));
        }
    }
}
