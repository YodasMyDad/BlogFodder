﻿using System;
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
                name: "BlogFodderPosts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    DateCreated = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DateUpdated = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Excerpt = table.Column<string>(type: "TEXT", maxLength: 3000, nullable: true),
                    AuthorId = table.Column<int>(type: "INTEGER", nullable: false),
                    FeaturedImage = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    PageTitle = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    MetaDescription = table.Column<string>(type: "TEXT", maxLength: 350, nullable: true),
                    NoIndex = table.Column<bool>(type: "INTEGER", nullable: false),
                    SocialImage = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    Url = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    ExtendedData = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlogFodderPosts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BlogFodderRoles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlogFodderRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BlogFodderUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    UserName = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "INTEGER", nullable: false),
                    PasswordHash = table.Column<string>(type: "TEXT", nullable: true),
                    SecurityStamp = table.Column<string>(type: "TEXT", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "TEXT", nullable: true),
                    PhoneNumber = table.Column<string>(type: "TEXT", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "INTEGER", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlogFodderUsers", x => x.Id);
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
                    PluginSettings = table.Column<string>(type: "TEXT", nullable: true),
                    Selector = table.Column<string>(type: "TEXT", nullable: true),
                    GlobalSettings = table.Column<string>(type: "TEXT", nullable: true)
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
                name: "BlogFodderRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RoleId = table.Column<Guid>(type: "TEXT", nullable: false),
                    ClaimType = table.Column<string>(type: "TEXT", nullable: true),
                    ClaimValue = table.Column<string>(type: "TEXT", nullable: true)
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
                name: "BlogFodderUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<Guid>(type: "TEXT", nullable: false),
                    ClaimType = table.Column<string>(type: "TEXT", nullable: true),
                    ClaimValue = table.Column<string>(type: "TEXT", nullable: true)
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
                    LoginProvider = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                    ProviderKey = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "TEXT", nullable: true),
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
                    LoginProvider = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                    Value = table.Column<string>(type: "TEXT", nullable: true)
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

            migrationBuilder.CreateIndex(
                name: "IX_BlogFodderPostContentItems_PostId",
                table: "BlogFodderPostContentItems",
                column: "PostId");

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
                name: "UserNameIndex",
                table: "BlogFodderUsers",
                column: "NormalizedUserName",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BlogFodderPostContentItems");

            migrationBuilder.DropTable(
                name: "BlogFodderRoleClaims");

            migrationBuilder.DropTable(
                name: "BlogFodderUserClaims");

            migrationBuilder.DropTable(
                name: "BlogFodderUserLogins");

            migrationBuilder.DropTable(
                name: "BlogFodderUserRoles");

            migrationBuilder.DropTable(
                name: "BlogFodderUserTokens");

            migrationBuilder.DropTable(
                name: "BlogFodderPosts");

            migrationBuilder.DropTable(
                name: "BlogFodderRoles");

            migrationBuilder.DropTable(
                name: "BlogFodderUsers");
        }
    }
}