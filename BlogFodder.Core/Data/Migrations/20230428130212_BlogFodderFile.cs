using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlogFodder.Core.Data.Migrations
{
    /// <inheritdoc />
    public partial class BlogFodderFile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BlogFodderFiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Url = table.Column<string>(type: "TEXT", maxLength: 600, nullable: true),
                    FileType = table.Column<int>(type: "INTEGER", nullable: false),
                    ItemId = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    FileSize = table.Column<long>(type: "INTEGER", nullable: false),
                    IsTemp = table.Column<bool>(type: "INTEGER", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ExtendedData = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlogFodderFiles", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BlogFodderFileDateCreated",
                table: "BlogFodderFiles",
                column: "DateCreated");

            migrationBuilder.CreateIndex(
                name: "IX_BlogFodderFileIsTemp",
                table: "BlogFodderFiles",
                column: "IsTemp");

            migrationBuilder.CreateIndex(
                name: "IX_BlogFodderFileItemId",
                table: "BlogFodderFiles",
                column: "ItemId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BlogFodderFiles");
        }
    }
}
