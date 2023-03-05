using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectFinalEngineer.Migrations
{
    public partial class initdb3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "ReactCount",
                table: "RoomingHouse",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "ReactCount",
                table: "Post",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "ReactCount",
                table: "Knowledge",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "ViewCount",
                table: "Knowledge",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReactCount",
                table: "RoomingHouse");

            migrationBuilder.DropColumn(
                name: "ReactCount",
                table: "Post");

            migrationBuilder.DropColumn(
                name: "ReactCount",
                table: "Knowledge");

            migrationBuilder.DropColumn(
                name: "ViewCount",
                table: "Knowledge");
        }
    }
}
