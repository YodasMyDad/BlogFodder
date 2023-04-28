using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlogFodder.Core.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddingImagesToBlog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FeaturedImage",
                table: "BlogFodderPosts");

            migrationBuilder.DropColumn(
                name: "SocialImage",
                table: "BlogFodderPosts");

            migrationBuilder.AddColumn<Guid>(
                name: "FeaturedImageId",
                table: "BlogFodderPosts",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "SocialImageId",
                table: "BlogFodderPosts",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BlogFodderPosts_FeaturedImageId",
                table: "BlogFodderPosts",
                column: "FeaturedImageId");

            migrationBuilder.CreateIndex(
                name: "IX_BlogFodderPosts_SocialImageId",
                table: "BlogFodderPosts",
                column: "SocialImageId");

            migrationBuilder.AddForeignKey(
                name: "FK_BlogFodderPosts_BlogFodderFiles_FeaturedImageId",
                table: "BlogFodderPosts",
                column: "FeaturedImageId",
                principalTable: "BlogFodderFiles",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BlogFodderPosts_BlogFodderFiles_SocialImageId",
                table: "BlogFodderPosts",
                column: "SocialImageId",
                principalTable: "BlogFodderFiles",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BlogFodderPosts_BlogFodderFiles_FeaturedImageId",
                table: "BlogFodderPosts");

            migrationBuilder.DropForeignKey(
                name: "FK_BlogFodderPosts_BlogFodderFiles_SocialImageId",
                table: "BlogFodderPosts");

            migrationBuilder.DropIndex(
                name: "IX_BlogFodderPosts_FeaturedImageId",
                table: "BlogFodderPosts");

            migrationBuilder.DropIndex(
                name: "IX_BlogFodderPosts_SocialImageId",
                table: "BlogFodderPosts");

            migrationBuilder.DropColumn(
                name: "FeaturedImageId",
                table: "BlogFodderPosts");

            migrationBuilder.DropColumn(
                name: "SocialImageId",
                table: "BlogFodderPosts");

            migrationBuilder.AddColumn<string>(
                name: "FeaturedImage",
                table: "BlogFodderPosts",
                type: "TEXT",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SocialImage",
                table: "BlogFodderPosts",
                type: "TEXT",
                maxLength: 1000,
                nullable: true);
        }
    }
}
