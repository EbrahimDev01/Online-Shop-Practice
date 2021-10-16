using Microsoft.EntityFrameworkCore.Migrations;

namespace MyEshop.Data.Migrations
{
    public partial class AddNameImageToTableImage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NameImage",
                table: "Images",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NameImage",
                table: "Images");
        }
    }
}
