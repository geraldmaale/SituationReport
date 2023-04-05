﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SR.Data.Migrations
{
    public partial class UpdatedOperationOffice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Role",
                table: "OperationOffices",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Role",
                table: "OperationOffices");
        }
    }
}
