using Microsoft.EntityFrameworkCore.Migrations;

namespace BlindDateBot.Data.Migrations
{
    public partial class CreateUsersTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    user_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    username_telegram_id = table.Column<int>(type: "int", nullable: false),
                    username = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    user_gender = table.Column<int>(type: "int", nullable: false),
                    interlocuter_gender = table.Column<int>(type: "int", nullable: false),
                    is_free = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("user_pkey", x => x.user_id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
