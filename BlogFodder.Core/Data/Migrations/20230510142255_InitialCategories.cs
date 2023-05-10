using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlogFodder.Core.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCategories : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BlogFodderCategories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    SortOrder = table.Column<int>(type: "INTEGER", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DateUpdated = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CategoryImageId = table.Column<Guid>(type: "TEXT", nullable: true),
                    PageTitle = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    MetaDescription = table.Column<string>(type: "TEXT", maxLength: 350, nullable: true),
                    NoIndex = table.Column<bool>(type: "INTEGER", nullable: false),
                    SocialImageId = table.Column<Guid>(type: "TEXT", nullable: true),
                    Url = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    ExtendedData = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlogFodderCategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BlogFodderCategories_BlogFodderFiles_CategoryImageId",
                        column: x => x.CategoryImageId,
                        principalTable: "BlogFodderFiles",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_BlogFodderCategories_BlogFodderFiles_SocialImageId",
                        column: x => x.SocialImageId,
                        principalTable: "BlogFodderFiles",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CategoryPost",
                columns: table => new
                {
                    CategoriesId = table.Column<Guid>(type: "TEXT", nullable: false),
                    PostsId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryPost", x => new { x.CategoriesId, x.PostsId });
                    table.ForeignKey(
                        name: "FK_CategoryPost_BlogFodderCategories_CategoriesId",
                        column: x => x.CategoriesId,
                        principalTable: "BlogFodderCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CategoryPost_BlogFodderPosts_PostsId",
                        column: x => x.PostsId,
                        principalTable: "BlogFodderPosts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BlogFodderCategories_CategoryImageId",
                table: "BlogFodderCategories",
                column: "CategoryImageId");

            migrationBuilder.CreateIndex(
                name: "IX_BlogFodderCategories_SocialImageId",
                table: "BlogFodderCategories",
                column: "SocialImageId");

            migrationBuilder.CreateIndex(
                name: "IX_CategoryPost_PostsId",
                table: "CategoryPost",
                column: "PostsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CategoryPost");

            migrationBuilder.DropTable(
                name: "BlogFodderCategories");
        }
    }
}
