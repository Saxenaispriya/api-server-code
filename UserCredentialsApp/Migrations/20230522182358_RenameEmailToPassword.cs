using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UserCredentialsApp.Migrations
{
    public partial class RenameEmailToPassword : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
            name: "email",
            table: "userRegisters",
            newName: "password");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
            name: "password",
            table: "userRegisters",
            newName: "email");
        }
    }
}
