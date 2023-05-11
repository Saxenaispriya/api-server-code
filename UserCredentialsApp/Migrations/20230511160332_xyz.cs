using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UserCredentialsApp.Migrations
{
    public partial class xyz : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
           name: "name",
           table: "userRegisters",
           newName: "username");

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
           name: "username",
           table: "userRegisters",
           newName: "name");
        }
    }
}
