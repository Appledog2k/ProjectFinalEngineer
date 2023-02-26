using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ProjectFinalEngineer.Migrations
{
    public partial class initdb1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Category_Slug",
                table: "Category");

            migrationBuilder.DropColumn(
                name: "Slug",
                table: "Category");

            migrationBuilder.AddColumn<int>(
                name: "KnowledgeId",
                table: "PostCategory",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ViewCount",
                table: "Post",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<int>(
                name: "RoomingHouseId",
                table: "Comment",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Area",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    ParentAreaId = table.Column<int>(type: "integer", nullable: true),
                    ParentCategoryId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Area", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Area_Area_ParentCategoryId",
                        column: x => x.ParentCategoryId,
                        principalTable: "Area",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Knowledge",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "character varying(160)", maxLength: 160, nullable: false),
                    Content = table.Column<string>(type: "text", nullable: true),
                    AuthorId = table.Column<string>(type: "text", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    DateUpdated = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Knowledge", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Knowledge_Users_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RoomingHouse",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "character varying(160)", maxLength: 160, nullable: false),
                    Content = table.Column<string>(type: "text", nullable: true),
                    Price = table.Column<float>(type: "real", nullable: false),
                    Published = table.Column<bool>(type: "boolean", nullable: false),
                    ApproverId = table.Column<string>(type: "text", nullable: true),
                    AuthorId = table.Column<string>(type: "text", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    DateUpdated = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    ViewCount = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoomingHouse", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoomingHouse_Users_ApproverId",
                        column: x => x.ApproverId,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RoomingHouse_Users_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "KnowledgeCategory",
                columns: table => new
                {
                    KnowledgeID = table.Column<int>(type: "integer", nullable: false),
                    CategoryID = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KnowledgeCategory", x => new { x.KnowledgeID, x.CategoryID });
                    table.ForeignKey(
                        name: "FK_KnowledgeCategory_Category_CategoryID",
                        column: x => x.CategoryID,
                        principalTable: "Category",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_KnowledgeCategory_Knowledge_KnowledgeID",
                        column: x => x.KnowledgeID,
                        principalTable: "Knowledge",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RommingHouseArea",
                columns: table => new
                {
                    AreaID = table.Column<int>(type: "integer", nullable: false),
                    RommingHouseID = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RommingHouseArea", x => new { x.AreaID, x.RommingHouseID });
                    table.ForeignKey(
                        name: "FK_RommingHouseArea_Area_AreaID",
                        column: x => x.AreaID,
                        principalTable: "Area",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RommingHouseArea_RoomingHouse_RommingHouseID",
                        column: x => x.RommingHouseID,
                        principalTable: "RoomingHouse",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PostCategory_KnowledgeId",
                table: "PostCategory",
                column: "KnowledgeId");

            migrationBuilder.CreateIndex(
                name: "IX_Comment_RoomingHouseId",
                table: "Comment",
                column: "RoomingHouseId");

            migrationBuilder.CreateIndex(
                name: "IX_Area_ParentCategoryId",
                table: "Area",
                column: "ParentCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Knowledge_AuthorId",
                table: "Knowledge",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_KnowledgeCategory_CategoryID",
                table: "KnowledgeCategory",
                column: "CategoryID");

            migrationBuilder.CreateIndex(
                name: "IX_RommingHouseArea_RommingHouseID",
                table: "RommingHouseArea",
                column: "RommingHouseID");

            migrationBuilder.CreateIndex(
                name: "IX_RoomingHouse_ApproverId",
                table: "RoomingHouse",
                column: "ApproverId");

            migrationBuilder.CreateIndex(
                name: "IX_RoomingHouse_AuthorId",
                table: "RoomingHouse",
                column: "AuthorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comment_RoomingHouse_RoomingHouseId",
                table: "Comment",
                column: "RoomingHouseId",
                principalTable: "RoomingHouse",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PostCategory_Knowledge_KnowledgeId",
                table: "PostCategory",
                column: "KnowledgeId",
                principalTable: "Knowledge",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comment_RoomingHouse_RoomingHouseId",
                table: "Comment");

            migrationBuilder.DropForeignKey(
                name: "FK_PostCategory_Knowledge_KnowledgeId",
                table: "PostCategory");

            migrationBuilder.DropTable(
                name: "KnowledgeCategory");

            migrationBuilder.DropTable(
                name: "RommingHouseArea");

            migrationBuilder.DropTable(
                name: "Knowledge");

            migrationBuilder.DropTable(
                name: "Area");

            migrationBuilder.DropTable(
                name: "RoomingHouse");

            migrationBuilder.DropIndex(
                name: "IX_PostCategory_KnowledgeId",
                table: "PostCategory");

            migrationBuilder.DropIndex(
                name: "IX_Comment_RoomingHouseId",
                table: "Comment");

            migrationBuilder.DropColumn(
                name: "KnowledgeId",
                table: "PostCategory");

            migrationBuilder.DropColumn(
                name: "ViewCount",
                table: "Post");

            migrationBuilder.DropColumn(
                name: "RoomingHouseId",
                table: "Comment");

            migrationBuilder.AddColumn<string>(
                name: "Slug",
                table: "Category",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Category_Slug",
                table: "Category",
                column: "Slug",
                unique: true);
        }
    }
}
