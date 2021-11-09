using Microsoft.EntityFrameworkCore.Migrations;

namespace Programemein.Data.Migrations
{
    public partial class AddSourceModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SourceId",
                table: "Memes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Source",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TypeName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OriginalUrl = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Source", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Memes_SourceId",
                table: "Memes",
                column: "SourceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Memes_Source_SourceId",
                table: "Memes",
                column: "SourceId",
                principalTable: "Source",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Memes_Source_SourceId",
                table: "Memes");

            migrationBuilder.DropTable(
                name: "Source");

            migrationBuilder.DropIndex(
                name: "IX_Memes_SourceId",
                table: "Memes");

            migrationBuilder.DropColumn(
                name: "SourceId",
                table: "Memes");
        }
    }
}
