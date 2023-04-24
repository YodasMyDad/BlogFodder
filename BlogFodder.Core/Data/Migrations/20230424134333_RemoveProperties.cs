using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlogFodder.Core.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemoveProperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GlobalSettings",
                table: "BlogFodderPostContentItems");

            migrationBuilder.DropColumn(
                name: "Selector",
                table: "BlogFodderPostContentItems");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "GlobalSettings",
                table: "BlogFodderPostContentItems",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Selector",
                table: "BlogFodderPostContentItems",
                type: "TEXT",
                nullable: true);
        }
    }
}
