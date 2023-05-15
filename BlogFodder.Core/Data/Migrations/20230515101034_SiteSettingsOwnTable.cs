using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlogFodder.Core.Data.Migrations
{
    /// <inheritdoc />
    public partial class SiteSettingsOwnTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BlogFodderSiteSettings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    DateUpdated = table.Column<DateTime>(type: "TEXT", nullable: false),
                    LogoId = table.Column<Guid>(type: "TEXT", nullable: true),
                    SiteName = table.Column<string>(type: "TEXT", maxLength: 400, nullable: false),
                    DefaultPageTitle = table.Column<string>(type: "TEXT", maxLength: 150, nullable: false),
                    DefaultMetaDescription = table.Column<string>(type: "TEXT", maxLength: 400, nullable: false),
                    HomeAmountPerPage = table.Column<int>(type: "INTEGER", nullable: false),
                    ExtendedData = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlogFodderSiteSettings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BlogFodderSiteSettings_BlogFodderFiles_LogoId",
                        column: x => x.LogoId,
                        principalTable: "BlogFodderFiles",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_BlogFodderSiteSettings_LogoId",
                table: "BlogFodderSiteSettings",
                column: "LogoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BlogFodderSiteSettings");
        }
    }
}
