using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlogFodder.Core.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdatingIdentity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ExtendedData",
                table: "BlogFodderUsers",
                type: "nvarchar(3500)",
                maxLength: 3500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "ProfileImageId",
                table: "BlogFodderUsers",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "RoleId1",
                table: "BlogFodderUserRoles",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "UserId1",
                table: "BlogFodderUserRoles",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "BlogFodderRoles",
                type: "TEXT",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExtendedData",
                table: "BlogFodderRoles",
                type: "nvarchar(3000)",
                maxLength: 3000,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "BlogFodderPosts",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BlogFodderUsers_ProfileImageId",
                table: "BlogFodderUsers",
                column: "ProfileImageId");

            migrationBuilder.CreateIndex(
                name: "IX_BlogFodderUserRoles_RoleId1",
                table: "BlogFodderUserRoles",
                column: "RoleId1");

            migrationBuilder.CreateIndex(
                name: "IX_BlogFodderUserRoles_UserId1",
                table: "BlogFodderUserRoles",
                column: "UserId1");

            migrationBuilder.CreateIndex(
                name: "IX_BlogFodderPosts_UserId",
                table: "BlogFodderPosts",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_BlogFodderPosts_BlogFodderUsers_UserId",
                table: "BlogFodderPosts",
                column: "UserId",
                principalTable: "BlogFodderUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BlogFodderUserRoles_BlogFodderRoles_RoleId1",
                table: "BlogFodderUserRoles",
                column: "RoleId1",
                principalTable: "BlogFodderRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BlogFodderUserRoles_BlogFodderUsers_UserId1",
                table: "BlogFodderUserRoles",
                column: "UserId1",
                principalTable: "BlogFodderUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BlogFodderUsers_BlogFodderFiles_ProfileImageId",
                table: "BlogFodderUsers",
                column: "ProfileImageId",
                principalTable: "BlogFodderFiles",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BlogFodderPosts_BlogFodderUsers_UserId",
                table: "BlogFodderPosts");

            migrationBuilder.DropForeignKey(
                name: "FK_BlogFodderUserRoles_BlogFodderRoles_RoleId1",
                table: "BlogFodderUserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_BlogFodderUserRoles_BlogFodderUsers_UserId1",
                table: "BlogFodderUserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_BlogFodderUsers_BlogFodderFiles_ProfileImageId",
                table: "BlogFodderUsers");

            migrationBuilder.DropIndex(
                name: "IX_BlogFodderUsers_ProfileImageId",
                table: "BlogFodderUsers");

            migrationBuilder.DropIndex(
                name: "IX_BlogFodderUserRoles_RoleId1",
                table: "BlogFodderUserRoles");

            migrationBuilder.DropIndex(
                name: "IX_BlogFodderUserRoles_UserId1",
                table: "BlogFodderUserRoles");

            migrationBuilder.DropIndex(
                name: "IX_BlogFodderPosts_UserId",
                table: "BlogFodderPosts");

            migrationBuilder.DropColumn(
                name: "ExtendedData",
                table: "BlogFodderUsers");

            migrationBuilder.DropColumn(
                name: "ProfileImageId",
                table: "BlogFodderUsers");

            migrationBuilder.DropColumn(
                name: "RoleId1",
                table: "BlogFodderUserRoles");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "BlogFodderUserRoles");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "BlogFodderRoles");

            migrationBuilder.DropColumn(
                name: "ExtendedData",
                table: "BlogFodderRoles");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "BlogFodderPosts");
        }
    }
}
