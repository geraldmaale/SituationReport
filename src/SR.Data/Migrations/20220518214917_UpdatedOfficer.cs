using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SR.Data.Migrations
{
    public partial class UpdatedOfficer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Phone",
                table: "Officers",
                newName: "Username");

            migrationBuilder.AddColumn<string>(
                name: "FullName",
                table: "Officers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "Officers",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FullName",
                table: "Officers");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "Officers");

            migrationBuilder.RenameColumn(
                name: "Username",
                table: "Officers",
                newName: "Phone");
        }
    }
}
