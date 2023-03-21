using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectFinalEngineer.Migrations
{
    public partial class initdb13 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RommingHouseArea");

            migrationBuilder.CreateTable(
                name: "RoomingHouseArea",
                columns: table => new
                {
                    RoomingHouseId = table.Column<int>(type: "integer", nullable: false),
                    AreaId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoomingHouseArea", x => new { x.RoomingHouseId, x.AreaId });
                    table.ForeignKey(
                        name: "FK_RoomingHouseArea_Area_AreaId",
                        column: x => x.AreaId,
                        principalTable: "Area",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RoomingHouseArea_RoomingHouse_RoomingHouseId",
                        column: x => x.RoomingHouseId,
                        principalTable: "RoomingHouse",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RoomingHouseArea_AreaId",
                table: "RoomingHouseArea",
                column: "AreaId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RoomingHouseArea");

            migrationBuilder.CreateTable(
                name: "RommingHouseArea",
                columns: table => new
                {
                    AreaId = table.Column<int>(type: "integer", nullable: false),
                    RommingHouseId = table.Column<int>(type: "integer", nullable: false),
                    AreaID = table.Column<int>(type: "integer", nullable: true),
                    RommingHouseID = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RommingHouseArea", x => new { x.AreaId, x.RommingHouseId });
                    table.ForeignKey(
                        name: "FK_RommingHouseArea_Area_AreaID",
                        column: x => x.AreaID,
                        principalTable: "Area",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RommingHouseArea_RoomingHouse_RommingHouseID",
                        column: x => x.RommingHouseID,
                        principalTable: "RoomingHouse",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_RommingHouseArea_AreaID",
                table: "RommingHouseArea",
                column: "AreaID");

            migrationBuilder.CreateIndex(
                name: "IX_RommingHouseArea_RommingHouseID",
                table: "RommingHouseArea",
                column: "RommingHouseID");
        }
    }
}
