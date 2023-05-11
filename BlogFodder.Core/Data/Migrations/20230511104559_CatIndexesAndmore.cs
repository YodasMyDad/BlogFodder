using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlogFodder.Core.Data.Migrations
{
    /// <inheritdoc />
    public partial class CatIndexesAndmore : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PostsPerPage",
                table: "BlogFodderCategories",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_PostUrl",
                table: "BlogFodderPosts",
                column: "Url");

            migrationBuilder.CreateIndex(
                name: "IX_CategoryUrl",
                table: "BlogFodderCategories",
                column: "Url");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PostUrl",
                table: "BlogFodderPosts");

            migrationBuilder.DropIndex(
                name: "IX_CategoryUrl",
                table: "BlogFodderCategories");

            migrationBuilder.DropColumn(
                name: "PostsPerPage",
                table: "BlogFodderCategories");
        }
    }
}
