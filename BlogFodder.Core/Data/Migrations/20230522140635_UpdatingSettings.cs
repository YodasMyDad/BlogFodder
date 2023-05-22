using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlogFodder.Core.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdatingSettings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PostId",
                table: "BlogFodderPlugins");

            migrationBuilder.AddColumn<string>(
                name: "Facebook",
                table: "BlogFodderSiteSettings",
                type: "TEXT",
                maxLength: 300,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Instagram",
                table: "BlogFodderSiteSettings",
                type: "TEXT",
                maxLength: 300,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LinkedIn",
                table: "BlogFodderSiteSettings",
                type: "TEXT",
                maxLength: 300,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Pinterest",
                table: "BlogFodderSiteSettings",
                type: "TEXT",
                maxLength: 300,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Twitter",
                table: "BlogFodderSiteSettings",
                type: "TEXT",
                maxLength: 300,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Facebook",
                table: "BlogFodderSiteSettings");

            migrationBuilder.DropColumn(
                name: "Instagram",
                table: "BlogFodderSiteSettings");

            migrationBuilder.DropColumn(
                name: "LinkedIn",
                table: "BlogFodderSiteSettings");

            migrationBuilder.DropColumn(
                name: "Pinterest",
                table: "BlogFodderSiteSettings");

            migrationBuilder.DropColumn(
                name: "Twitter",
                table: "BlogFodderSiteSettings");

            migrationBuilder.AddColumn<Guid>(
                name: "PostId",
                table: "BlogFodderPlugins",
                type: "TEXT",
                nullable: true);
        }
    }
}
