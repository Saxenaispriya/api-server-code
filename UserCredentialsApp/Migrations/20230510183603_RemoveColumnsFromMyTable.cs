using Microsoft.EntityFrameworkCore.Migrations;
using System.Numerics;

#nullable disable

namespace UserCredentialsApp.Migrations
{
    public partial class RemoveColumnsFromMyTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.DropColumn(
                   name: "Phone",
                   table: "userRegisters");


        }



        protected override void Down(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.AddColumn<int>(
              name: "Email",
              table: "userRegisters",
               nullable: false,
              defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                   name: "Phone",
                   table: "userRegisters");
        }
    }
}
