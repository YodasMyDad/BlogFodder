using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlogFodder.Core.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixingUpdatingIdentity4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BlogFodderUserRoles_BlogFodderUsers_UserId1",
                table: "BlogFodderUserRoles");

            migrationBuilder.DropIndex(
                name: "IX_BlogFodderUserRoles_UserId1",
                table: "BlogFodderUserRoles");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "BlogFodderUserRoles");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "UserId1",
                table: "BlogFodderUserRoles",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_BlogFodderUserRoles_UserId1",
                table: "BlogFodderUserRoles",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_BlogFodderUserRoles_BlogFodderUsers_UserId1",
                table: "BlogFodderUserRoles",
                column: "UserId1",
                principalTable: "BlogFodderUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
