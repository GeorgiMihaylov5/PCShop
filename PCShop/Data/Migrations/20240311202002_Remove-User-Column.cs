using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PCShop.Data.Migrations
{
    public partial class RemoveUserColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MyProperty",
                table: "Users");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MyProperty",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
