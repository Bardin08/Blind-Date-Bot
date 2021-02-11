using Microsoft.EntityFrameworkCore.Migrations;

namespace BlindDateBot.Data.Migrations
{
    public partial class AddVisibleField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "is_visible",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "is_visible",
                table: "Users");
        }
    }
}
