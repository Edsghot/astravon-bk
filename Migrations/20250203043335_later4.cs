using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Astragon.Migrations
{
    public partial class later4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Editor",
                table: "ScientificArticle");

            migrationBuilder.DropColumn(
                name: "Actions",
                table: "AcademicFormations");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Editor",
                table: "ScientificArticle",
                type: "varchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Actions",
                table: "AcademicFormations",
                type: "varchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}
