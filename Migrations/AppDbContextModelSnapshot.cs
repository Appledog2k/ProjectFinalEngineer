﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using ProjectFinalEngineer.EntityFramework;

#nullable disable

namespace ProjectFinalEngineer.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.12")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex");

                    b.ToTable("Roles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("text");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("RoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("text");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("UserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("text");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("text");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("text");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("UserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.Property<string>("RoleId")
                        .HasColumnType("text");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("UserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("Value")
                        .HasColumnType("text");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("UserTokens", (string)null);
                });

            modelBuilder.Entity("ProjectFinalEngineer.Models.AggregateCategory.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<int?>("ParentCategoryId")
                        .HasColumnType("integer");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.HasKey("Id");

                    b.HasIndex("ParentCategoryId");

                    b.ToTable("Category");
                });

            modelBuilder.Entity("ProjectFinalEngineer.Models.AggregateComment.Comment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("AuthorId")
                        .HasColumnType("text");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int?>("ParentId")
                        .HasColumnType("integer");

                    b.Property<int?>("PostId")
                        .HasColumnType("integer");

                    b.Property<int?>("RoomingHouseId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.HasIndex("ParentId");

                    b.HasIndex("PostId");

                    b.HasIndex("RoomingHouseId");

                    b.ToTable("Comment");
                });

            modelBuilder.Entity("ProjectFinalEngineer.Models.AggregateContact.Contact", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime?>("DateSent")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar");

                    b.Property<string>("Message")
                        .HasColumnType("text");

                    b.Property<string>("Phone")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.HasKey("Id");

                    b.ToTable("Contacts");
                });

            modelBuilder.Entity("ProjectFinalEngineer.Models.AggregateKnowledge.Knowledge", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("AuthorId")
                        .HasColumnType("text");

                    b.Property<string>("Content")
                        .HasColumnType("text");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime>("DateUpdated")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Media")
                        .HasColumnType("text");

                    b.Property<long>("ReactCount")
                        .HasColumnType("bigint");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(160)
                        .HasColumnType("character varying(160)");

                    b.Property<long>("ViewCount")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.ToTable("Knowledge");
                });

            modelBuilder.Entity("ProjectFinalEngineer.Models.AggregateKnowledge.KnowledgeCategory", b =>
                {
                    b.Property<int>("KnowledgeId")
                        .HasColumnType("integer");

                    b.Property<int>("CategoryId")
                        .HasColumnType("integer");

                    b.Property<int?>("CategoryID")
                        .HasColumnType("integer");

                    b.Property<int?>("KnowledgeID")
                        .HasColumnType("integer");

                    b.HasKey("KnowledgeId", "CategoryId");

                    b.HasIndex("CategoryID");

                    b.HasIndex("KnowledgeID");

                    b.ToTable("KnowledgeCategory");
                });

            modelBuilder.Entity("ProjectFinalEngineer.Models.AggregatePost.Post", b =>
                {
                    b.Property<int>("PostId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("PostId"));

                    b.Property<string>("ApproverId")
                        .HasColumnType("text");

                    b.Property<string>("AuthorId")
                        .HasColumnType("text");

                    b.Property<string>("Content")
                        .HasColumnType("text");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime>("DateUpdated")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Media")
                        .HasColumnType("text");

                    b.Property<int>("Priority")
                        .HasColumnType("integer");

                    b.Property<bool>("Published")
                        .HasColumnType("boolean");

                    b.Property<long>("ReactCount")
                        .HasColumnType("bigint");

                    b.Property<string>("Reason")
                        .HasColumnType("text");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(160)
                        .HasColumnType("character varying(160)");

                    b.Property<long>("ViewCount")
                        .HasColumnType("bigint");

                    b.HasKey("PostId");

                    b.HasIndex("ApproverId");

                    b.HasIndex("AuthorId");

                    b.ToTable("Post");
                });

            modelBuilder.Entity("ProjectFinalEngineer.Models.AggregatePostCategory.PostCategory", b =>
                {
                    b.Property<int>("PostId")
                        .HasColumnType("integer");

                    b.Property<int>("CategoryId")
                        .HasColumnType("integer");

                    b.HasKey("PostId", "CategoryId");

                    b.HasIndex("CategoryId");

                    b.ToTable("PostCategory");
                });

            modelBuilder.Entity("ProjectFinalEngineer.Models.AggregateUser.AppUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("integer");

                    b.Property<string>("Avatar")
                        .HasColumnType("text");

                    b.Property<DateTime?>("BirthDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("text");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("boolean");

                    b.Property<string>("FullName")
                        .HasColumnType("text");

                    b.Property<string>("HomeAdress")
                        .HasMaxLength(400)
                        .HasColumnType("varchar");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("boolean");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("text");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("text");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("boolean");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("text");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("boolean");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex");

                    b.ToTable("Users", (string)null);
                });

            modelBuilder.Entity("ProjectFinalEngineer.Models.RoomingHouse.Area", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<int?>("ParentAreaId")
                        .HasColumnType("integer");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.HasKey("Id");

                    b.HasIndex("ParentAreaId");

                    b.ToTable("Area");
                });

            modelBuilder.Entity("ProjectFinalEngineer.Models.RoomingHouse.RoomingHouse", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ApproverId")
                        .HasColumnType("text");

                    b.Property<string>("AuthorId")
                        .HasColumnType("text");

                    b.Property<string>("Content")
                        .HasColumnType("text");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime>("DateUpdated")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("Image")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<float>("Price")
                        .HasColumnType("real");

                    b.Property<int>("Priority")
                        .HasColumnType("integer");

                    b.Property<bool>("Published")
                        .HasColumnType("boolean");

                    b.Property<long>("ReactCount")
                        .HasColumnType("bigint");

                    b.Property<string>("Reason")
                        .HasColumnType("text");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(160)
                        .HasColumnType("character varying(160)");

                    b.Property<long>("ViewCount")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("ApproverId");

                    b.HasIndex("AuthorId");

                    b.ToTable("RoomingHouse");
                });

            modelBuilder.Entity("ProjectFinalEngineer.Models.RoomingHouse.RoomingHouseArea", b =>
                {
                    b.Property<int>("RoomingHouseId")
                        .HasColumnType("integer");

                    b.Property<int>("AreaId")
                        .HasColumnType("integer");

                    b.HasKey("RoomingHouseId", "AreaId");

                    b.HasIndex("AreaId");

                    b.ToTable("RoomingHouseArea");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("ProjectFinalEngineer.Models.AggregateUser.AppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("ProjectFinalEngineer.Models.AggregateUser.AppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ProjectFinalEngineer.Models.AggregateUser.AppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("ProjectFinalEngineer.Models.AggregateUser.AppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ProjectFinalEngineer.Models.AggregateCategory.Category", b =>
                {
                    b.HasOne("ProjectFinalEngineer.Models.AggregateCategory.Category", "ParentCategory")
                        .WithMany("CategoryChildren")
                        .HasForeignKey("ParentCategoryId");

                    b.Navigation("ParentCategory");
                });

            modelBuilder.Entity("ProjectFinalEngineer.Models.AggregateComment.Comment", b =>
                {
                    b.HasOne("ProjectFinalEngineer.Models.AggregateUser.AppUser", "Author")
                        .WithMany()
                        .HasForeignKey("AuthorId");

                    b.HasOne("ProjectFinalEngineer.Models.AggregateComment.Comment", "Parent")
                        .WithMany("Comments")
                        .HasForeignKey("ParentId");

                    b.HasOne("ProjectFinalEngineer.Models.AggregatePost.Post", "Post")
                        .WithMany("Comments")
                        .HasForeignKey("PostId");

                    b.HasOne("ProjectFinalEngineer.Models.RoomingHouse.RoomingHouse", null)
                        .WithMany("Comments")
                        .HasForeignKey("RoomingHouseId");

                    b.Navigation("Author");

                    b.Navigation("Parent");

                    b.Navigation("Post");
                });

            modelBuilder.Entity("ProjectFinalEngineer.Models.AggregateKnowledge.Knowledge", b =>
                {
                    b.HasOne("ProjectFinalEngineer.Models.AggregateUser.AppUser", "Author")
                        .WithMany()
                        .HasForeignKey("AuthorId");

                    b.Navigation("Author");
                });

            modelBuilder.Entity("ProjectFinalEngineer.Models.AggregateKnowledge.KnowledgeCategory", b =>
                {
                    b.HasOne("ProjectFinalEngineer.Models.AggregateCategory.Category", "Category")
                        .WithMany()
                        .HasForeignKey("CategoryID");

                    b.HasOne("ProjectFinalEngineer.Models.AggregateKnowledge.Knowledge", "Knowledge")
                        .WithMany("KnowledgeCategories")
                        .HasForeignKey("KnowledgeID");

                    b.Navigation("Category");

                    b.Navigation("Knowledge");
                });

            modelBuilder.Entity("ProjectFinalEngineer.Models.AggregatePost.Post", b =>
                {
                    b.HasOne("ProjectFinalEngineer.Models.AggregateUser.AppUser", "Approver")
                        .WithMany()
                        .HasForeignKey("ApproverId");

                    b.HasOne("ProjectFinalEngineer.Models.AggregateUser.AppUser", "Author")
                        .WithMany()
                        .HasForeignKey("AuthorId");

                    b.Navigation("Approver");

                    b.Navigation("Author");
                });

            modelBuilder.Entity("ProjectFinalEngineer.Models.AggregatePostCategory.PostCategory", b =>
                {
                    b.HasOne("ProjectFinalEngineer.Models.AggregateCategory.Category", "Category")
                        .WithMany()
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ProjectFinalEngineer.Models.AggregatePost.Post", "Post")
                        .WithMany("PostCategories")
                        .HasForeignKey("PostId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");

                    b.Navigation("Post");
                });

            modelBuilder.Entity("ProjectFinalEngineer.Models.RoomingHouse.Area", b =>
                {
                    b.HasOne("ProjectFinalEngineer.Models.RoomingHouse.Area", "ParentArea")
                        .WithMany("AreaChildren")
                        .HasForeignKey("ParentAreaId");

                    b.Navigation("ParentArea");
                });

            modelBuilder.Entity("ProjectFinalEngineer.Models.RoomingHouse.RoomingHouse", b =>
                {
                    b.HasOne("ProjectFinalEngineer.Models.AggregateUser.AppUser", "Approver")
                        .WithMany()
                        .HasForeignKey("ApproverId");

                    b.HasOne("ProjectFinalEngineer.Models.AggregateUser.AppUser", "Author")
                        .WithMany()
                        .HasForeignKey("AuthorId");

                    b.Navigation("Approver");

                    b.Navigation("Author");
                });

            modelBuilder.Entity("ProjectFinalEngineer.Models.RoomingHouse.RoomingHouseArea", b =>
                {
                    b.HasOne("ProjectFinalEngineer.Models.RoomingHouse.Area", "Area")
                        .WithMany()
                        .HasForeignKey("AreaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ProjectFinalEngineer.Models.RoomingHouse.RoomingHouse", "RoomingHouse")
                        .WithMany("RoomingHouseAreas")
                        .HasForeignKey("RoomingHouseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Area");

                    b.Navigation("RoomingHouse");
                });

            modelBuilder.Entity("ProjectFinalEngineer.Models.AggregateCategory.Category", b =>
                {
                    b.Navigation("CategoryChildren");
                });

            modelBuilder.Entity("ProjectFinalEngineer.Models.AggregateComment.Comment", b =>
                {
                    b.Navigation("Comments");
                });

            modelBuilder.Entity("ProjectFinalEngineer.Models.AggregateKnowledge.Knowledge", b =>
                {
                    b.Navigation("KnowledgeCategories");
                });

            modelBuilder.Entity("ProjectFinalEngineer.Models.AggregatePost.Post", b =>
                {
                    b.Navigation("Comments");

                    b.Navigation("PostCategories");
                });

            modelBuilder.Entity("ProjectFinalEngineer.Models.RoomingHouse.Area", b =>
                {
                    b.Navigation("AreaChildren");
                });

            modelBuilder.Entity("ProjectFinalEngineer.Models.RoomingHouse.RoomingHouse", b =>
                {
                    b.Navigation("Comments");

                    b.Navigation("RoomingHouseAreas");
                });
#pragma warning restore 612, 618
        }
    }
}
