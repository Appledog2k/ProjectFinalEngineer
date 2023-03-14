using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectFinalEngineer.Migrations
{
    public partial class initdb9 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "RoomingHouse",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image",
                table: "RoomingHouse");
        }
    }
}
