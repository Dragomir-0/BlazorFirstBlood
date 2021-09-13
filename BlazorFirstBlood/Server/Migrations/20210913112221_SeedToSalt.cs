using Microsoft.EntityFrameworkCore.Migrations;

namespace BlazorFirstBlood.Server.Migrations
{
    public partial class SeedToSalt : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PasswordSeed",
                table: "Users",
                newName: "PasswordSalt");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PasswordSalt",
                table: "Users",
                newName: "PasswordSeed");
        }
    }
}
