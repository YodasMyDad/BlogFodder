using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlogFodder.Core.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemoveCategoryImage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BlogFodderCategories_BlogFodderFiles_CategoryImageId",
                table: "BlogFodderCategories");

            migrationBuilder.DropIndex(
                name: "IX_BlogFodderCategories_CategoryImageId",
                table: "BlogFodderCategories");

            migrationBuilder.DropColumn(
                name: "CategoryImageId",
                table: "BlogFodderCategories");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CategoryImageId",
                table: "BlogFodderCategories",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BlogFodderCategories_CategoryImageId",
                table: "BlogFodderCategories",
                column: "CategoryImageId");

            migrationBuilder.AddForeignKey(
                name: "FK_BlogFodderCategories_BlogFodderFiles_CategoryImageId",
                table: "BlogFodderCategories",
                column: "CategoryImageId",
                principalTable: "BlogFodderFiles",
                principalColumn: "Id");
        }
    }
}
