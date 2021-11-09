using Microsoft.EntityFrameworkCore.Migrations;

namespace Programemein.Data.Migrations
{
    public partial class RenameSourceToSources : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Memes_Source_SourceId",
                table: "Memes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Source",
                table: "Source");

            migrationBuilder.RenameTable(
                name: "Source",
                newName: "Sources");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Sources",
                table: "Sources",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Memes_Sources_SourceId",
                table: "Memes",
                column: "SourceId",
                principalTable: "Sources",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Memes_Sources_SourceId",
                table: "Memes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Sources",
                table: "Sources");

            migrationBuilder.RenameTable(
                name: "Sources",
                newName: "Source");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Source",
                table: "Source",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Memes_Source_SourceId",
                table: "Memes",
                column: "SourceId",
                principalTable: "Source",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
