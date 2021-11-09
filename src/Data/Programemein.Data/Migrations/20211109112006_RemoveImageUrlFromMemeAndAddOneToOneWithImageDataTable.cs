using Microsoft.EntityFrameworkCore.Migrations;

namespace Programemein.Data.Migrations
{
    public partial class RemoveImageUrlFromMemeAndAddOneToOneWithImageDataTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Memes");

            migrationBuilder.AddColumn<int>(
                name: "MemeId",
                table: "Images",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Images_MemeId",
                table: "Images",
                column: "MemeId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Images_Memes_MemeId",
                table: "Images",
                column: "MemeId",
                principalTable: "Memes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Images_Memes_MemeId",
                table: "Images");

            migrationBuilder.DropIndex(
                name: "IX_Images_MemeId",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "MemeId",
                table: "Images");

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Memes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
