using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectFinalEngineer.Migrations
{
    public partial class initdb2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostCategory_Knowledge_KnowledgeId",
                table: "PostCategory");

            migrationBuilder.DropIndex(
                name: "IX_PostCategory_KnowledgeId",
                table: "PostCategory");

            migrationBuilder.DropColumn(
                name: "KnowledgeId",
                table: "PostCategory");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "KnowledgeId",
                table: "PostCategory",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PostCategory_KnowledgeId",
                table: "PostCategory",
                column: "KnowledgeId");

            migrationBuilder.AddForeignKey(
                name: "FK_PostCategory_Knowledge_KnowledgeId",
                table: "PostCategory",
                column: "KnowledgeId",
                principalTable: "Knowledge",
                principalColumn: "Id");
        }
    }
}
