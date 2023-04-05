using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SR.Data.Migrations
{
    public partial class UpdatedOfficer2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CrewProcessings_Officers_OfficerId",
                table: "CrewProcessings");

            migrationBuilder.DropForeignKey(
                name: "FK_OperationInspections_Officers_OfficerId",
                table: "OperationInspections");

            migrationBuilder.DropForeignKey(
                name: "FK_PassportProcessings_Officers_OfficerId",
                table: "PassportProcessings");

            migrationBuilder.DropForeignKey(
                name: "FK_PermitProcessings_Officers_OfficerId",
                table: "PermitProcessings");

            migrationBuilder.DropForeignKey(
                name: "FK_RevenueCollections_Officers_OfficerId",
                table: "RevenueCollections");

            migrationBuilder.AddForeignKey(
                name: "FK_CrewProcessings_Officers_OfficerId",
                table: "CrewProcessings",
                column: "OfficerId",
                principalTable: "Officers",
                principalColumn: "OfficerId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OperationInspections_Officers_OfficerId",
                table: "OperationInspections",
                column: "OfficerId",
                principalTable: "Officers",
                principalColumn: "OfficerId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PassportProcessings_Officers_OfficerId",
                table: "PassportProcessings",
                column: "OfficerId",
                principalTable: "Officers",
                principalColumn: "OfficerId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PermitProcessings_Officers_OfficerId",
                table: "PermitProcessings",
                column: "OfficerId",
                principalTable: "Officers",
                principalColumn: "OfficerId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RevenueCollections_Officers_OfficerId",
                table: "RevenueCollections",
                column: "OfficerId",
                principalTable: "Officers",
                principalColumn: "OfficerId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CrewProcessings_Officers_OfficerId",
                table: "CrewProcessings");

            migrationBuilder.DropForeignKey(
                name: "FK_OperationInspections_Officers_OfficerId",
                table: "OperationInspections");

            migrationBuilder.DropForeignKey(
                name: "FK_PassportProcessings_Officers_OfficerId",
                table: "PassportProcessings");

            migrationBuilder.DropForeignKey(
                name: "FK_PermitProcessings_Officers_OfficerId",
                table: "PermitProcessings");

            migrationBuilder.DropForeignKey(
                name: "FK_RevenueCollections_Officers_OfficerId",
                table: "RevenueCollections");

            migrationBuilder.AddForeignKey(
                name: "FK_CrewProcessings_Officers_OfficerId",
                table: "CrewProcessings",
                column: "OfficerId",
                principalTable: "Officers",
                principalColumn: "OfficerId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OperationInspections_Officers_OfficerId",
                table: "OperationInspections",
                column: "OfficerId",
                principalTable: "Officers",
                principalColumn: "OfficerId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PassportProcessings_Officers_OfficerId",
                table: "PassportProcessings",
                column: "OfficerId",
                principalTable: "Officers",
                principalColumn: "OfficerId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PermitProcessings_Officers_OfficerId",
                table: "PermitProcessings",
                column: "OfficerId",
                principalTable: "Officers",
                principalColumn: "OfficerId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RevenueCollections_Officers_OfficerId",
                table: "RevenueCollections",
                column: "OfficerId",
                principalTable: "Officers",
                principalColumn: "OfficerId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
