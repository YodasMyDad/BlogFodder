using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlogFodder.Core.Data.Migrations
{
    /// <inheritdoc />
    public partial class DateonRolesAndUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreateDate",
                table: "BlogFodderUsers",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreateDate",
                table: "BlogFodderRoles",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreateDate",
                table: "BlogFodderUsers");

            migrationBuilder.DropColumn(
                name: "CreateDate",
                table: "BlogFodderRoles");
        }
    }
}
