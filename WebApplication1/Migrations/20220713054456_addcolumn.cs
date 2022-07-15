using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication1.Migrations
{
    public partial class addcolumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastActive",
                table: "ApplicationUser");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "ApplicationUser",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "UserGuid",
                table: "ApplicationUser",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<bool>(
                name: "isActive",
                table: "ApplicationUser",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "ApplicationUser");

            migrationBuilder.DropColumn(
                name: "UserGuid",
                table: "ApplicationUser");

            migrationBuilder.DropColumn(
                name: "isActive",
                table: "ApplicationUser");

            migrationBuilder.AddColumn<DateTime>(
                name: "LastActive",
                table: "ApplicationUser",
                type: "datetime2",
                nullable: true);
        }
    }
}
