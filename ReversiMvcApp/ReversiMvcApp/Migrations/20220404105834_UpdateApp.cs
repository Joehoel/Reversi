using Microsoft.EntityFrameworkCore.Migrations;

namespace ReversiMvcApp.Migrations
{
    public partial class UpdateApp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "GamesWon",
                table: "Players",
                newName: "Wins");

            migrationBuilder.RenameColumn(
                name: "GamesTied",
                table: "Players",
                newName: "Losses");

            migrationBuilder.RenameColumn(
                name: "GamesLost",
                table: "Players",
                newName: "Draws");

            migrationBuilder.AddColumn<int>(
                name: "Color",
                table: "Players",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Players",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Color",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Players");

            migrationBuilder.RenameColumn(
                name: "Wins",
                table: "Players",
                newName: "GamesWon");

            migrationBuilder.RenameColumn(
                name: "Losses",
                table: "Players",
                newName: "GamesTied");

            migrationBuilder.RenameColumn(
                name: "Draws",
                table: "Players",
                newName: "GamesLost");
        }
    }
}
