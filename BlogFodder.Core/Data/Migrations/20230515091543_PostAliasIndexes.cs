using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlogFodder.Core.Data.Migrations
{
    /// <inheritdoc />
    public partial class PostAliasIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_PostPluginPluginAlias",
                table: "BlogFodderPostPlugins",
                column: "PluginAlias");

            migrationBuilder.CreateIndex(
                name: "IX_PostContentItemPluginAlias",
                table: "BlogFodderPostContentItems",
                column: "PluginAlias");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PostPluginPluginAlias",
                table: "BlogFodderPostPlugins");

            migrationBuilder.DropIndex(
                name: "IX_PostContentItemPluginAlias",
                table: "BlogFodderPostContentItems");
        }
    }
}
