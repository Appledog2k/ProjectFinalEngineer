using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectFinalEngineer.Migrations
{
    public partial class initdb10 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Area_Area_ParentCategoryId",
                table: "Area");

            migrationBuilder.DropForeignKey(
                name: "FK_KnowledgeCategory_Category_CategoryID",
                table: "KnowledgeCategory");

            migrationBuilder.DropForeignKey(
                name: "FK_KnowledgeCategory_Knowledge_KnowledgeID",
                table: "KnowledgeCategory");

            migrationBuilder.DropForeignKey(
                name: "FK_PostCategory_Category_CategoryID",
                table: "PostCategory");

            migrationBuilder.DropForeignKey(
                name: "FK_PostCategory_Post_PostID",
                table: "PostCategory");

            migrationBuilder.DropForeignKey(
                name: "FK_RommingHouseArea_Area_AreaID",
                table: "RommingHouseArea");

            migrationBuilder.DropForeignKey(
                name: "FK_RommingHouseArea_RoomingHouse_RommingHouseID",
                table: "RommingHouseArea");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RommingHouseArea",
                table: "RommingHouseArea");

            migrationBuilder.DropPrimaryKey(
                name: "PK_KnowledgeCategory",
                table: "KnowledgeCategory");

            migrationBuilder.DropIndex(
                name: "IX_Area_ParentCategoryId",
                table: "Area");

            migrationBuilder.DropColumn(
                name: "ParentCategoryId",
                table: "Area");

            migrationBuilder.RenameColumn(
                name: "CategoryID",
                table: "PostCategory",
                newName: "CategoryId");

            migrationBuilder.RenameColumn(
                name: "PostID",
                table: "PostCategory",
                newName: "PostId");

            migrationBuilder.RenameIndex(
                name: "IX_PostCategory_CategoryID",
                table: "PostCategory",
                newName: "IX_PostCategory_CategoryId");

            migrationBuilder.AlterColumn<string>(
                name: "Image",
                table: "RoomingHouse",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "RommingHouseID",
                table: "RommingHouseArea",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "AreaID",
                table: "RommingHouseArea",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<int>(
                name: "AreaId",
                table: "RommingHouseArea",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RommingHouseId",
                table: "RommingHouseArea",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "CategoryID",
                table: "KnowledgeCategory",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "KnowledgeID",
                table: "KnowledgeCategory",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<int>(
                name: "KnowledgeId",
                table: "KnowledgeCategory",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "KnowledgeCategory",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "Comment",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_RommingHouseArea",
                table: "RommingHouseArea",
                columns: new[] { "AreaId", "RommingHouseId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_KnowledgeCategory",
                table: "KnowledgeCategory",
                columns: new[] { "KnowledgeId", "CategoryId" });

            migrationBuilder.CreateIndex(
                name: "IX_RommingHouseArea_AreaID",
                table: "RommingHouseArea",
                column: "AreaID");

            migrationBuilder.CreateIndex(
                name: "IX_KnowledgeCategory_KnowledgeID",
                table: "KnowledgeCategory",
                column: "KnowledgeID");

            migrationBuilder.CreateIndex(
                name: "IX_Area_ParentAreaId",
                table: "Area",
                column: "ParentAreaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Area_Area_ParentAreaId",
                table: "Area",
                column: "ParentAreaId",
                principalTable: "Area",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_KnowledgeCategory_Category_CategoryID",
                table: "KnowledgeCategory",
                column: "CategoryID",
                principalTable: "Category",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_KnowledgeCategory_Knowledge_KnowledgeID",
                table: "KnowledgeCategory",
                column: "KnowledgeID",
                principalTable: "Knowledge",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PostCategory_Category_CategoryId",
                table: "PostCategory",
                column: "CategoryId",
                principalTable: "Category",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PostCategory_Post_PostId",
                table: "PostCategory",
                column: "PostId",
                principalTable: "Post",
                principalColumn: "PostId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RommingHouseArea_Area_AreaID",
                table: "RommingHouseArea",
                column: "AreaID",
                principalTable: "Area",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RommingHouseArea_RoomingHouse_RommingHouseID",
                table: "RommingHouseArea",
                column: "RommingHouseID",
                principalTable: "RoomingHouse",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Area_Area_ParentAreaId",
                table: "Area");

            migrationBuilder.DropForeignKey(
                name: "FK_KnowledgeCategory_Category_CategoryID",
                table: "KnowledgeCategory");

            migrationBuilder.DropForeignKey(
                name: "FK_KnowledgeCategory_Knowledge_KnowledgeID",
                table: "KnowledgeCategory");

            migrationBuilder.DropForeignKey(
                name: "FK_PostCategory_Category_CategoryId",
                table: "PostCategory");

            migrationBuilder.DropForeignKey(
                name: "FK_PostCategory_Post_PostId",
                table: "PostCategory");

            migrationBuilder.DropForeignKey(
                name: "FK_RommingHouseArea_Area_AreaID",
                table: "RommingHouseArea");

            migrationBuilder.DropForeignKey(
                name: "FK_RommingHouseArea_RoomingHouse_RommingHouseID",
                table: "RommingHouseArea");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RommingHouseArea",
                table: "RommingHouseArea");

            migrationBuilder.DropIndex(
                name: "IX_RommingHouseArea_AreaID",
                table: "RommingHouseArea");

            migrationBuilder.DropPrimaryKey(
                name: "PK_KnowledgeCategory",
                table: "KnowledgeCategory");

            migrationBuilder.DropIndex(
                name: "IX_KnowledgeCategory_KnowledgeID",
                table: "KnowledgeCategory");

            migrationBuilder.DropIndex(
                name: "IX_Area_ParentAreaId",
                table: "Area");

            migrationBuilder.DropColumn(
                name: "AreaId",
                table: "RommingHouseArea");

            migrationBuilder.DropColumn(
                name: "RommingHouseId",
                table: "RommingHouseArea");

            migrationBuilder.DropColumn(
                name: "KnowledgeId",
                table: "KnowledgeCategory");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "KnowledgeCategory");

            migrationBuilder.RenameColumn(
                name: "CategoryId",
                table: "PostCategory",
                newName: "CategoryID");

            migrationBuilder.RenameColumn(
                name: "PostId",
                table: "PostCategory",
                newName: "PostID");

            migrationBuilder.RenameIndex(
                name: "IX_PostCategory_CategoryId",
                table: "PostCategory",
                newName: "IX_PostCategory_CategoryID");

            migrationBuilder.AlterColumn<string>(
                name: "Image",
                table: "RoomingHouse",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<int>(
                name: "RommingHouseID",
                table: "RommingHouseArea",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "AreaID",
                table: "RommingHouseArea",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "KnowledgeID",
                table: "KnowledgeCategory",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CategoryID",
                table: "KnowledgeCategory",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "Comment",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<int>(
                name: "ParentCategoryId",
                table: "Area",
                type: "integer",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_RommingHouseArea",
                table: "RommingHouseArea",
                columns: new[] { "AreaID", "RommingHouseID" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_KnowledgeCategory",
                table: "KnowledgeCategory",
                columns: new[] { "KnowledgeID", "CategoryID" });

            migrationBuilder.CreateIndex(
                name: "IX_Area_ParentCategoryId",
                table: "Area",
                column: "ParentCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Area_Area_ParentCategoryId",
                table: "Area",
                column: "ParentCategoryId",
                principalTable: "Area",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_KnowledgeCategory_Category_CategoryID",
                table: "KnowledgeCategory",
                column: "CategoryID",
                principalTable: "Category",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_KnowledgeCategory_Knowledge_KnowledgeID",
                table: "KnowledgeCategory",
                column: "KnowledgeID",
                principalTable: "Knowledge",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PostCategory_Category_CategoryID",
                table: "PostCategory",
                column: "CategoryID",
                principalTable: "Category",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PostCategory_Post_PostID",
                table: "PostCategory",
                column: "PostID",
                principalTable: "Post",
                principalColumn: "PostId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RommingHouseArea_Area_AreaID",
                table: "RommingHouseArea",
                column: "AreaID",
                principalTable: "Area",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RommingHouseArea_RoomingHouse_RommingHouseID",
                table: "RommingHouseArea",
                column: "RommingHouseID",
                principalTable: "RoomingHouse",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
