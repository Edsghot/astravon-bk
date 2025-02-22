using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Astragon.Migrations
{
    public partial class later2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Doi",
                table: "ResearchProject");

            migrationBuilder.DropColumn(
                name: "Editor",
                table: "ResearchProject");

            migrationBuilder.AddColumn<int>(
                name: "Estatus",
                table: "ScientificArticle",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Estatus",
                table: "ScientificArticle");

            migrationBuilder.AddColumn<string>(
                name: "Doi",
                table: "ResearchProject",
                type: "varchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Editor",
                table: "ResearchProject",
                type: "varchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}
