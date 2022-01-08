using Microsoft.EntityFrameworkCore.Migrations;

namespace Programemein.Data.Migrations
{
    public partial class AddIsInInstagramFieldToMemeModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsInInstagram",
                table: "Memes",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsInInstagram",
                table: "Memes");
        }
    }
}
