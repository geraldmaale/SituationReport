using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SR.Data.Migrations
{
    public partial class UpdatedOperations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OperationProcessingDetails");

            migrationBuilder.DropTable(
                name: "OperationsProcessings");

            migrationBuilder.CreateTable(
                name: "OperationInspections",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Total = table.Column<int>(type: "integer", nullable: false),
                    OperationOfficeId = table.Column<Guid>(type: "uuid", nullable: false),
                    EntryDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    OfficerId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OperationInspections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OperationInspections_Officers_OfficerId",
                        column: x => x.OfficerId,
                        principalTable: "Officers",
                        principalColumn: "OfficerId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OperationInspections_OperationOffices_OperationOfficeId",
                        column: x => x.OperationOfficeId,
                        principalTable: "OperationOffices",
                        principalColumn: "OperationOfficeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OperationInspectionDetails",
                columns: table => new
                {
                    OperationInspectionDetailId = table.Column<Guid>(type: "uuid", nullable: false),
                    Number = table.Column<int>(type: "integer", nullable: false),
                    OperationTypeId = table.Column<int>(type: "integer", nullable: false),
                    OperationInspectionId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OperationInspectionDetails", x => x.OperationInspectionDetailId);
                    table.ForeignKey(
                        name: "FK_OperationInspectionDetails_OperationInspections_OperationIn~",
                        column: x => x.OperationInspectionId,
                        principalTable: "OperationInspections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OperationInspectionDetails_OperationTypes_OperationTypeId",
                        column: x => x.OperationTypeId,
                        principalTable: "OperationTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OperationInspectionDetails_OperationInspectionId",
                table: "OperationInspectionDetails",
                column: "OperationInspectionId");

            migrationBuilder.CreateIndex(
                name: "IX_OperationInspectionDetails_OperationTypeId",
                table: "OperationInspectionDetails",
                column: "OperationTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_OperationInspections_OfficerId",
                table: "OperationInspections",
                column: "OfficerId");

            migrationBuilder.CreateIndex(
                name: "IX_OperationInspections_OperationOfficeId",
                table: "OperationInspections",
                column: "OperationOfficeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OperationInspectionDetails");

            migrationBuilder.DropTable(
                name: "OperationInspections");

            migrationBuilder.CreateTable(
                name: "OperationsProcessings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OfficerId = table.Column<Guid>(type: "uuid", nullable: false),
                    OperationOfficeId = table.Column<Guid>(type: "uuid", nullable: false),
                    EntryDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Total = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OperationsProcessings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OperationsProcessings_Officers_OfficerId",
                        column: x => x.OfficerId,
                        principalTable: "Officers",
                        principalColumn: "OfficerId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OperationsProcessings_OperationOffices_OperationOfficeId",
                        column: x => x.OperationOfficeId,
                        principalTable: "OperationOffices",
                        principalColumn: "OperationOfficeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OperationProcessingDetails",
                columns: table => new
                {
                    OperationProcessingDetailId = table.Column<Guid>(type: "uuid", nullable: false),
                    OperationProcessingId = table.Column<Guid>(type: "uuid", nullable: false),
                    OperationTypeId = table.Column<int>(type: "integer", nullable: false),
                    Number = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OperationProcessingDetails", x => x.OperationProcessingDetailId);
                    table.ForeignKey(
                        name: "FK_OperationProcessingDetails_OperationsProcessings_OperationP~",
                        column: x => x.OperationProcessingId,
                        principalTable: "OperationsProcessings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OperationProcessingDetails_OperationTypes_OperationTypeId",
                        column: x => x.OperationTypeId,
                        principalTable: "OperationTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OperationProcessingDetails_OperationProcessingId",
                table: "OperationProcessingDetails",
                column: "OperationProcessingId");

            migrationBuilder.CreateIndex(
                name: "IX_OperationProcessingDetails_OperationTypeId",
                table: "OperationProcessingDetails",
                column: "OperationTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_OperationsProcessings_OfficerId",
                table: "OperationsProcessings",
                column: "OfficerId");

            migrationBuilder.CreateIndex(
                name: "IX_OperationsProcessings_OperationOfficeId",
                table: "OperationsProcessings",
                column: "OperationOfficeId");
        }
    }
}
