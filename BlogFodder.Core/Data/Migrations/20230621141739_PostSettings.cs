using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlogFodder.Core.Data.Migrations
{
    /// <inheritdoc />
    public partial class PostSettings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ShowAuthorName",
                table: "BlogFodderPosts",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ShowDefaultHeading",
                table: "BlogFodderPosts",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ShowLastUpdated",
                table: "BlogFodderPosts",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShowAuthorName",
                table: "BlogFodderPosts");

            migrationBuilder.DropColumn(
                name: "ShowDefaultHeading",
                table: "BlogFodderPosts");

            migrationBuilder.DropColumn(
                name: "ShowLastUpdated",
                table: "BlogFodderPosts");
        }
    }
}
