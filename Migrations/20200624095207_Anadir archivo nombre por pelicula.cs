using Microsoft.EntityFrameworkCore.Migrations;

namespace DonLEonFilms.Migrations
{
    public partial class Anadirarchivonombreporpelicula : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FileName",
                table: "Films",
                maxLength: 512,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileName",
                table: "Films");
        }
    }
}
