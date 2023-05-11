using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UserCredentialsApp.Migrations
{
    public partial class abc : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
       name: "Phone",
       table: "userRegisters");

           
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
           
        }
    }
}
