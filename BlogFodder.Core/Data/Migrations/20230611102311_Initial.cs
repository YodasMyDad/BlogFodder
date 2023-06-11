using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlogFodder.Core.Data.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
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

            migrationBuilder.CreateTable(
                name: "BlogFodderPlugins",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    PluginAlias = table.Column<string>(type: "TEXT", maxLength: 600, nullable: false),
                    PluginData = table.Column<string>(type: "TEXT", nullable: true),
                    PluginSettings = table.Column<string>(type: "TEXT", nullable: true),
                    Enabled = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlogFodderPlugins", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BlogFodderPluginsGlobalSettings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Alias = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    Data = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlogFodderPluginsGlobalSettings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BlogFodderRoles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    CreateDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ExtendedData = table.Column<string>(type: "nvarchar(3000)", maxLength: 3000, nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "TEXT", maxLength: 3000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlogFodderRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BlogFodderCategories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    SortOrder = table.Column<int>(type: "INTEGER", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DateUpdated = table.Column<DateTime>(type: "TEXT", nullable: false),
                    PostsPerPage = table.Column<int>(type: "INTEGER", nullable: false),
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
                        name: "FK_BlogFodderCategories_BlogFodderFiles_SocialImageId",
                        column: x => x.SocialImageId,
                        principalTable: "BlogFodderFiles",
                        principalColumn: "Id");
                });

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
                    Instagram = table.Column<string>(type: "TEXT", maxLength: 300, nullable: true),
                    Twitter = table.Column<string>(type: "TEXT", maxLength: 300, nullable: true),
                    Facebook = table.Column<string>(type: "TEXT", maxLength: 300, nullable: true),
                    Pinterest = table.Column<string>(type: "TEXT", maxLength: 300, nullable: true),
                    LinkedIn = table.Column<string>(type: "TEXT", maxLength: 300, nullable: true),
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

            migrationBuilder.CreateTable(
                name: "BlogFodderUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    ProfileImageId = table.Column<Guid>(type: "TEXT", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ExtendedData = table.Column<string>(type: "nvarchar(3500)", maxLength: 3500, nullable: false),
                    UserName = table.Column<string>(type: "TEXT", maxLength: 150, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "INTEGER", nullable: false),
                    PasswordHash = table.Column<string>(type: "TEXT", maxLength: 300, nullable: true),
                    SecurityStamp = table.Column<string>(type: "TEXT", maxLength: 3000, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "TEXT", maxLength: 3000, nullable: true),
                    PhoneNumber = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "INTEGER", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlogFodderUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BlogFodderUsers_BlogFodderFiles_ProfileImageId",
                        column: x => x.ProfileImageId,
                        principalTable: "BlogFodderFiles",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "BlogFodderRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RoleId = table.Column<Guid>(type: "TEXT", nullable: false),
                    ClaimType = table.Column<string>(type: "TEXT", maxLength: 3000, nullable: true),
                    ClaimValue = table.Column<string>(type: "TEXT", maxLength: 3000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlogFodderRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BlogFodderRoleClaims_BlogFodderRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "BlogFodderRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BlogFodderPosts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    DateCreated = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DateUpdated = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Excerpt = table.Column<string>(type: "TEXT", maxLength: 3000, nullable: true),
                    AuthorId = table.Column<int>(type: "INTEGER", nullable: false),
                    FeaturedImageId = table.Column<Guid>(type: "TEXT", nullable: true),
                    PageTitle = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    MetaDescription = table.Column<string>(type: "TEXT", maxLength: 350, nullable: true),
                    NoIndex = table.Column<bool>(type: "INTEGER", nullable: false),
                    SocialImageId = table.Column<Guid>(type: "TEXT", nullable: true),
                    Url = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    ExtendedData = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    UserId = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlogFodderPosts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BlogFodderPosts_BlogFodderFiles_FeaturedImageId",
                        column: x => x.FeaturedImageId,
                        principalTable: "BlogFodderFiles",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_BlogFodderPosts_BlogFodderFiles_SocialImageId",
                        column: x => x.SocialImageId,
                        principalTable: "BlogFodderFiles",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_BlogFodderPosts_BlogFodderUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "BlogFodderUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "BlogFodderUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<Guid>(type: "TEXT", nullable: false),
                    ClaimType = table.Column<string>(type: "TEXT", maxLength: 3000, nullable: true),
                    ClaimValue = table.Column<string>(type: "TEXT", maxLength: 3000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlogFodderUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BlogFodderUserClaims_BlogFodderUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "BlogFodderUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BlogFodderUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "TEXT", nullable: false),
                    ProviderKey = table.Column<string>(type: "TEXT", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "TEXT", maxLength: 3000, nullable: true),
                    UserId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlogFodderUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_BlogFodderUserLogins_BlogFodderUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "BlogFodderUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BlogFodderUserRoles",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "TEXT", nullable: false),
                    RoleId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlogFodderUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_BlogFodderUserRoles_BlogFodderRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "BlogFodderRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BlogFodderUserRoles_BlogFodderUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "BlogFodderUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BlogFodderUserTokens",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "TEXT", nullable: false),
                    LoginProvider = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Value = table.Column<string>(type: "TEXT", maxLength: 4000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlogFodderUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_BlogFodderUserTokens_BlogFodderUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "BlogFodderUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BlogFodderPostContentItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    PostId = table.Column<Guid>(type: "TEXT", nullable: false),
                    SortOrder = table.Column<int>(type: "INTEGER", nullable: false),
                    PluginAlias = table.Column<string>(type: "TEXT", maxLength: 600, nullable: true),
                    PluginData = table.Column<string>(type: "TEXT", nullable: true),
                    PluginSettings = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlogFodderPostContentItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BlogFodderPostContentItems_BlogFodderPosts_PostId",
                        column: x => x.PostId,
                        principalTable: "BlogFodderPosts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                name: "IX_BlogFodderCategories_SocialImageId",
                table: "BlogFodderCategories",
                column: "SocialImageId");

            migrationBuilder.CreateIndex(
                name: "IX_CategoryUrl",
                table: "BlogFodderCategories",
                column: "Url");

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

            migrationBuilder.CreateIndex(
                name: "IX_PluginAlias",
                table: "BlogFodderPlugins",
                column: "PluginAlias");

            migrationBuilder.CreateIndex(
                name: "IX_GlobalSettingsAlias",
                table: "BlogFodderPluginsGlobalSettings",
                column: "Alias");

            migrationBuilder.CreateIndex(
                name: "IX_BlogFodderPostContentItems_PostId",
                table: "BlogFodderPostContentItems",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_PostContentItemPluginAlias",
                table: "BlogFodderPostContentItems",
                column: "PluginAlias");

            migrationBuilder.CreateIndex(
                name: "IX_BlogFodderPosts_FeaturedImageId",
                table: "BlogFodderPosts",
                column: "FeaturedImageId");

            migrationBuilder.CreateIndex(
                name: "IX_BlogFodderPosts_SocialImageId",
                table: "BlogFodderPosts",
                column: "SocialImageId");

            migrationBuilder.CreateIndex(
                name: "IX_BlogFodderPosts_UserId",
                table: "BlogFodderPosts",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_PostUrl",
                table: "BlogFodderPosts",
                column: "Url");

            migrationBuilder.CreateIndex(
                name: "IX_BlogFodderRoleClaims_RoleId",
                table: "BlogFodderRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "BlogFodderRoles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BlogFodderSiteSettings_LogoId",
                table: "BlogFodderSiteSettings",
                column: "LogoId");

            migrationBuilder.CreateIndex(
                name: "IX_BlogFodderUserClaims_UserId",
                table: "BlogFodderUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_BlogFodderUserLogins_UserId",
                table: "BlogFodderUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_BlogFodderUserRoles_RoleId",
                table: "BlogFodderUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "BlogFodderUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "IX_BlogFodderUsers_ProfileImageId",
                table: "BlogFodderUsers",
                column: "ProfileImageId");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "BlogFodderUsers",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CategoryPost_PostsId",
                table: "CategoryPost",
                column: "PostsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BlogFodderPlugins");

            migrationBuilder.DropTable(
                name: "BlogFodderPluginsGlobalSettings");

            migrationBuilder.DropTable(
                name: "BlogFodderPostContentItems");

            migrationBuilder.DropTable(
                name: "BlogFodderRoleClaims");

            migrationBuilder.DropTable(
                name: "BlogFodderSiteSettings");

            migrationBuilder.DropTable(
                name: "BlogFodderUserClaims");

            migrationBuilder.DropTable(
                name: "BlogFodderUserLogins");

            migrationBuilder.DropTable(
                name: "BlogFodderUserRoles");

            migrationBuilder.DropTable(
                name: "BlogFodderUserTokens");

            migrationBuilder.DropTable(
                name: "CategoryPost");

            migrationBuilder.DropTable(
                name: "BlogFodderRoles");

            migrationBuilder.DropTable(
                name: "BlogFodderCategories");

            migrationBuilder.DropTable(
                name: "BlogFodderPosts");

            migrationBuilder.DropTable(
                name: "BlogFodderUsers");

            migrationBuilder.DropTable(
                name: "BlogFodderFiles");
        }
    }
}
