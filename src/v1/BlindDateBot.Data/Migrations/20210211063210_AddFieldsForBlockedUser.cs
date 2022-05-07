using Microsoft.EntityFrameworkCore.Migrations;

namespace BlindDateBot.Data.Migrations
{
    public partial class AddFieldsForBlockedUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ComplaintsAmount",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "block_reason",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "is_blocked",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ComplaintsAmount",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "block_reason",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "is_blocked",
                table: "Users");
        }
    }
}
