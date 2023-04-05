using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SR.Data.Migrations
{
    public partial class UpdatedAshorePass : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "AshorePasses",
                newName: "AshorePassId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_OfficerId",
                table: "AspNetUsers",
                column: "OfficerId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Officers_OfficerId",
                table: "AspNetUsers",
                column: "OfficerId",
                principalTable: "Officers",
                principalColumn: "OfficerId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Officers_OfficerId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_OfficerId",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "AshorePassId",
                table: "AshorePasses",
                newName: "Id");
        }
    }
}
