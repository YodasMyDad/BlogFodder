using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlogFodder.Core.Data.Migrations
{
    /// <inheritdoc />
    public partial class ReworkingPostPluginsToPlugins : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BlogFodderPostPlugins");

            migrationBuilder.CreateTable(
                name: "BlogFodderPlugins",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    PluginAlias = table.Column<string>(type: "TEXT", maxLength: 600, nullable: true),
                    PluginData = table.Column<string>(type: "TEXT", nullable: true),
                    PluginSettings = table.Column<string>(type: "TEXT", nullable: true),
                    PostId = table.Column<Guid>(type: "TEXT", nullable: true),
                    PluginDisplayAreas = table.Column<string>(type: "nvarchar(3000)", maxLength: 3000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlogFodderPlugins", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PluginAlias",
                table: "BlogFodderPlugins",
                column: "PluginAlias");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BlogFodderPlugins");

            migrationBuilder.CreateTable(
                name: "BlogFodderPostPlugins",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    PostId = table.Column<Guid>(type: "TEXT", nullable: false),
                    PluginAlias = table.Column<string>(type: "TEXT", maxLength: 600, nullable: true),
                    PluginData = table.Column<string>(type: "TEXT", nullable: true),
                    PluginDisplayArea = table.Column<int>(type: "INTEGER", nullable: false),
                    PluginSettings = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlogFodderPostPlugins", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BlogFodderPostPlugins_BlogFodderPosts_PostId",
                        column: x => x.PostId,
                        principalTable: "BlogFodderPosts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BlogFodderPostPlugins_PostId",
                table: "BlogFodderPostPlugins",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_PostPluginPluginAlias",
                table: "BlogFodderPostPlugins",
                column: "PluginAlias");
        }
    }
}
