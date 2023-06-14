﻿// <auto-generated />
using System;
using BlogFodder.Core.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace BlogFodder.Core.Data.Migrations
{
    [DbContext(typeof(BlogFodderDbContext))]
    [Migration("20230614145040_FixUserPostIssue")]
    partial class FixUserPostIssue
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.5");

            modelBuilder.Entity("BlogFodder.Core.Categories.Models.Category", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("DateCreated")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("DateUpdated")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("ExtendedData")
                        .IsRequired()
                        .HasMaxLength(4000)
                        .HasColumnType("nvarchar(4000)");

                    b.Property<string>("MetaDescription")
                        .HasMaxLength(350)
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasMaxLength(1000)
                        .HasColumnType("TEXT");

                    b.Property<bool>("NoIndex")
                        .HasColumnType("INTEGER");

                    b.Property<string>("PageTitle")
                        .HasMaxLength(100)
                        .HasColumnType("TEXT");

                    b.Property<int>("PostsPerPage")
                        .HasColumnType("INTEGER");

                    b.Property<Guid?>("SocialImageId")
                        .HasColumnType("TEXT");

                    b.Property<int>("SortOrder")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Url")
                        .HasMaxLength(500)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("SocialImageId");

                    b.HasIndex("Url")
                        .HasDatabaseName("IX_CategoryUrl");

                    b.ToTable("BlogFodderCategories", (string)null);
                });

            modelBuilder.Entity("BlogFodder.Core.Media.Models.BlogFodderFile", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("TEXT");

                    b.Property<string>("ExtendedData")
                        .IsRequired()
                        .HasMaxLength(4000)
                        .HasColumnType("nvarchar(4000)");

                    b.Property<long>("FileSize")
                        .HasColumnType("INTEGER");

                    b.Property<int>("FileType")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsTemp")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ItemId")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("TEXT");

                    b.Property<string>("Url")
                        .HasMaxLength(600)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("DateCreated")
                        .HasDatabaseName("IX_BlogFodderFileDateCreated");

                    b.HasIndex("IsTemp")
                        .HasDatabaseName("IX_BlogFodderFileIsTemp");

                    b.HasIndex("ItemId")
                        .HasDatabaseName("IX_BlogFodderFileItemId");

                    b.ToTable("BlogFodderFiles", (string)null);
                });

            modelBuilder.Entity("BlogFodder.Core.Membership.Models.Role", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasMaxLength(3000)
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .HasMaxLength(200)
                        .HasColumnType("TEXT");

                    b.Property<string>("ExtendedData")
                        .IsRequired()
                        .HasMaxLength(3000)
                        .HasColumnType("nvarchar(3000)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex");

                    b.ToTable("BlogFodderRoles", (string)null);
                });

            modelBuilder.Entity("BlogFodder.Core.Membership.Models.RoleClaim", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ClaimType")
                        .HasMaxLength(3000)
                        .HasColumnType("TEXT");

                    b.Property<string>("ClaimValue")
                        .HasMaxLength(3000)
                        .HasColumnType("TEXT");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("BlogFodderRoleClaims", (string)null);
                });

            modelBuilder.Entity("BlogFodder.Core.Membership.Models.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasMaxLength(3000)
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ExtendedData")
                        .IsRequired()
                        .HasMaxLength(3500)
                        .HasColumnType("nvarchar(3500)");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("INTEGER");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("TEXT");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<string>("PasswordHash")
                        .HasMaxLength(300)
                        .HasColumnType("TEXT");

                    b.Property<string>("PhoneNumber")
                        .HasMaxLength(100)
                        .HasColumnType("TEXT");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("INTEGER");

                    b.Property<Guid?>("ProfileImageId")
                        .HasColumnType("TEXT");

                    b.Property<string>("SecurityStamp")
                        .HasMaxLength(3000)
                        .HasColumnType("TEXT");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("INTEGER");

                    b.Property<string>("UserName")
                        .HasMaxLength(150)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex");

                    b.HasIndex("ProfileImageId");

                    b.ToTable("BlogFodderUsers", (string)null);
                });

            modelBuilder.Entity("BlogFodder.Core.Membership.Models.UserClaim", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ClaimType")
                        .HasMaxLength(3000)
                        .HasColumnType("TEXT");

                    b.Property<string>("ClaimValue")
                        .HasMaxLength(3000)
                        .HasColumnType("TEXT");

                    b.Property<Guid>("UserId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("BlogFodderUserClaims", (string)null);
                });

            modelBuilder.Entity("BlogFodder.Core.Membership.Models.UserLogin", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("TEXT");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("TEXT");

                    b.Property<string>("ProviderDisplayName")
                        .HasMaxLength(3000)
                        .HasColumnType("TEXT");

                    b.Property<Guid>("UserId")
                        .HasColumnType("TEXT");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("BlogFodderUserLogins", (string)null);
                });

            modelBuilder.Entity("BlogFodder.Core.Membership.Models.UserRole", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("TEXT");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("BlogFodderUserRoles", (string)null);
                });

            modelBuilder.Entity("BlogFodder.Core.Membership.Models.UserToken", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("TEXT");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<string>("Value")
                        .HasMaxLength(4000)
                        .HasColumnType("TEXT");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("BlogFodderUserTokens", (string)null);
                });

            modelBuilder.Entity("BlogFodder.Core.Plugins.Models.GlobalPluginSettings", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Alias")
                        .HasMaxLength(1000)
                        .HasColumnType("TEXT");

                    b.Property<string>("Data")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("Alias")
                        .HasDatabaseName("IX_GlobalSettingsAlias");

                    b.ToTable("BlogFodderPluginsGlobalSettings", (string)null);
                });

            modelBuilder.Entity("BlogFodder.Core.Plugins.Models.Plugin", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<bool>("Enabled")
                        .HasColumnType("INTEGER");

                    b.Property<string>("PluginAlias")
                        .IsRequired()
                        .HasMaxLength(600)
                        .HasColumnType("TEXT");

                    b.Property<string>("PluginData")
                        .HasColumnType("TEXT");

                    b.Property<string>("PluginSettings")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("PluginAlias")
                        .HasDatabaseName("IX_PluginAlias");

                    b.ToTable("BlogFodderPlugins", (string)null);
                });

            modelBuilder.Entity("BlogFodder.Core.Posts.Models.Post", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("DateCreated")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("DateUpdated")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Excerpt")
                        .HasMaxLength(3000)
                        .HasColumnType("TEXT");

                    b.Property<string>("ExtendedData")
                        .IsRequired()
                        .HasMaxLength(4000)
                        .HasColumnType("nvarchar(4000)");

                    b.Property<Guid?>("FeaturedImageId")
                        .HasColumnType("TEXT");

                    b.Property<string>("MetaDescription")
                        .HasMaxLength(350)
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasMaxLength(1000)
                        .HasColumnType("TEXT");

                    b.Property<bool>("NoIndex")
                        .HasColumnType("INTEGER");

                    b.Property<string>("PageTitle")
                        .HasMaxLength(100)
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("SocialImageId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Url")
                        .HasMaxLength(500)
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("UserId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("FeaturedImageId");

                    b.HasIndex("SocialImageId");

                    b.HasIndex("Url")
                        .HasDatabaseName("IX_PostUrl");

                    b.HasIndex("UserId");

                    b.ToTable("BlogFodderPosts", (string)null);
                });

            modelBuilder.Entity("BlogFodder.Core.Posts.Models.PostContentItem", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("PluginAlias")
                        .HasMaxLength(600)
                        .HasColumnType("TEXT");

                    b.Property<string>("PluginData")
                        .HasColumnType("TEXT");

                    b.Property<string>("PluginSettings")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("PostId")
                        .HasColumnType("TEXT");

                    b.Property<int>("SortOrder")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("PluginAlias")
                        .HasDatabaseName("IX_PostContentItemPluginAlias");

                    b.HasIndex("PostId");

                    b.ToTable("BlogFodderPostContentItems", (string)null);
                });

            modelBuilder.Entity("BlogFodder.Core.Settings.Models.SiteSettings", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("DateUpdated")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("DefaultMetaDescription")
                        .IsRequired()
                        .HasMaxLength(400)
                        .HasColumnType("TEXT");

                    b.Property<string>("DefaultPageTitle")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("TEXT");

                    b.Property<string>("ExtendedData")
                        .IsRequired()
                        .HasMaxLength(4000)
                        .HasColumnType("nvarchar(4000)");

                    b.Property<string>("Facebook")
                        .HasMaxLength(300)
                        .HasColumnType("TEXT");

                    b.Property<int>("HomeAmountPerPage")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Instagram")
                        .HasMaxLength(300)
                        .HasColumnType("TEXT");

                    b.Property<string>("LinkedIn")
                        .HasMaxLength(300)
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("LogoId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Pinterest")
                        .HasMaxLength(300)
                        .HasColumnType("TEXT");

                    b.Property<string>("SiteName")
                        .IsRequired()
                        .HasMaxLength(400)
                        .HasColumnType("TEXT");

                    b.Property<string>("Twitter")
                        .HasMaxLength(300)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("LogoId");

                    b.ToTable("BlogFodderSiteSettings", (string)null);
                });

            modelBuilder.Entity("CategoryPost", b =>
                {
                    b.Property<Guid>("CategoriesId")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("PostsId")
                        .HasColumnType("TEXT");

                    b.HasKey("CategoriesId", "PostsId");

                    b.HasIndex("PostsId");

                    b.ToTable("CategoryPost");
                });

            modelBuilder.Entity("BlogFodder.Core.Categories.Models.Category", b =>
                {
                    b.HasOne("BlogFodder.Core.Media.Models.BlogFodderFile", "SocialImage")
                        .WithMany()
                        .HasForeignKey("SocialImageId");

                    b.Navigation("SocialImage");
                });

            modelBuilder.Entity("BlogFodder.Core.Membership.Models.RoleClaim", b =>
                {
                    b.HasOne("BlogFodder.Core.Membership.Models.Role", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("BlogFodder.Core.Membership.Models.User", b =>
                {
                    b.HasOne("BlogFodder.Core.Media.Models.BlogFodderFile", "ProfileImage")
                        .WithMany()
                        .HasForeignKey("ProfileImageId");

                    b.Navigation("ProfileImage");
                });

            modelBuilder.Entity("BlogFodder.Core.Membership.Models.UserClaim", b =>
                {
                    b.HasOne("BlogFodder.Core.Membership.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("BlogFodder.Core.Membership.Models.UserLogin", b =>
                {
                    b.HasOne("BlogFodder.Core.Membership.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("BlogFodder.Core.Membership.Models.UserRole", b =>
                {
                    b.HasOne("BlogFodder.Core.Membership.Models.Role", "Role")
                        .WithMany("UserRoles")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BlogFodder.Core.Membership.Models.User", "User")
                        .WithMany("UserRoles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Role");

                    b.Navigation("User");
                });

            modelBuilder.Entity("BlogFodder.Core.Membership.Models.UserToken", b =>
                {
                    b.HasOne("BlogFodder.Core.Membership.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("BlogFodder.Core.Posts.Models.Post", b =>
                {
                    b.HasOne("BlogFodder.Core.Media.Models.BlogFodderFile", "FeaturedImage")
                        .WithMany()
                        .HasForeignKey("FeaturedImageId");

                    b.HasOne("BlogFodder.Core.Media.Models.BlogFodderFile", "SocialImage")
                        .WithMany()
                        .HasForeignKey("SocialImageId");

                    b.HasOne("BlogFodder.Core.Membership.Models.User", "User")
                        .WithMany("Posts")
                        .HasForeignKey("UserId");

                    b.Navigation("FeaturedImage");

                    b.Navigation("SocialImage");

                    b.Navigation("User");
                });

            modelBuilder.Entity("BlogFodder.Core.Posts.Models.PostContentItem", b =>
                {
                    b.HasOne("BlogFodder.Core.Posts.Models.Post", "Post")
                        .WithMany("ContentItems")
                        .HasForeignKey("PostId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Post");
                });

            modelBuilder.Entity("BlogFodder.Core.Settings.Models.SiteSettings", b =>
                {
                    b.HasOne("BlogFodder.Core.Media.Models.BlogFodderFile", "Logo")
                        .WithMany()
                        .HasForeignKey("LogoId");

                    b.Navigation("Logo");
                });

            modelBuilder.Entity("CategoryPost", b =>
                {
                    b.HasOne("BlogFodder.Core.Categories.Models.Category", null)
                        .WithMany()
                        .HasForeignKey("CategoriesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BlogFodder.Core.Posts.Models.Post", null)
                        .WithMany()
                        .HasForeignKey("PostsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("BlogFodder.Core.Membership.Models.Role", b =>
                {
                    b.Navigation("UserRoles");
                });

            modelBuilder.Entity("BlogFodder.Core.Membership.Models.User", b =>
                {
                    b.Navigation("Posts");

                    b.Navigation("UserRoles");
                });

            modelBuilder.Entity("BlogFodder.Core.Posts.Models.Post", b =>
                {
                    b.Navigation("ContentItems");
                });
#pragma warning restore 612, 618
        }
    }
}
