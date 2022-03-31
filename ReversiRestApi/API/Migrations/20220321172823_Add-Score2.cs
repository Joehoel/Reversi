using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Migrations
{
    public partial class AddScore2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BlackCount",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "WhiteCount",
                table: "Games");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BlackCount",
                table: "Games",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WhiteCount",
                table: "Games",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
