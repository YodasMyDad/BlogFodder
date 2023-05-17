using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlogFodder.Core.Data.Migrations
{
    /// <inheritdoc />
    public partial class ChangingPluginDataSaving : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PluginDisplayAreas",
                table: "BlogFodderPlugins");

            migrationBuilder.AlterColumn<string>(
                name: "PluginAlias",
                table: "BlogFodderPlugins",
                type: "TEXT",
                maxLength: 600,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 600,
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Enabled",
                table: "BlogFodderPlugins",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "PluginGlobalSettings",
                table: "BlogFodderPlugins",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Enabled",
                table: "BlogFodderPlugins");

            migrationBuilder.DropColumn(
                name: "PluginGlobalSettings",
                table: "BlogFodderPlugins");

            migrationBuilder.AlterColumn<string>(
                name: "PluginAlias",
                table: "BlogFodderPlugins",
                type: "TEXT",
                maxLength: 600,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 600);

            migrationBuilder.AddColumn<string>(
                name: "PluginDisplayAreas",
                table: "BlogFodderPlugins",
                type: "nvarchar(3000)",
                maxLength: 3000,
                nullable: false,
                defaultValue: "");
        }
    }
}
