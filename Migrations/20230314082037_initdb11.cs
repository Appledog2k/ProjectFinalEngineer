using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectFinalEngineer.Migrations
{
    public partial class initdb11 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Media",
                table: "Post",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Media",
                table: "Knowledge",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Media",
                table: "Post");

            migrationBuilder.DropColumn(
                name: "Media",
                table: "Knowledge");
        }
    }
}
