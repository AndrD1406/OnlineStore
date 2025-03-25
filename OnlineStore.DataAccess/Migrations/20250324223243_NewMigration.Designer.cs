﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using OnlineStore.DataAccess;

#nullable disable

namespace OnlineStore.DataAccess.Migrations
{
    [DbContext(typeof(OnlineStoreDbContext))]
    [Migration("20250324223243_NewMigration")]
    partial class NewMigration
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.13")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<System.Guid>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<System.Guid>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<System.Guid>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<System.Guid>", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<System.Guid>", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("OnlineStore.DataAccess.Models.ApplicationRole", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles", (string)null);

                    b.HasData(
                        new
                        {
                            Id = new Guid("292ee265-a479-4a3b-a222-e97b45c6d1a1"),
                            Name = "User",
                            NormalizedName = "USER"
                        },
                        new
                        {
                            Id = new Guid("eafe291d-eca5-4c09-90fe-4faba2fa479d"),
                            Name = "Admin",
                            NormalizedName = "ADMIN"
                        });
                });

            modelBuilder.Entity("OnlineStore.DataAccess.Models.ApplicationUser", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<bool>("IsAdmin")
                        .HasColumnType("bit");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("OnlineStore.DataAccess.Models.Cart", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Carts");
                });

            modelBuilder.Entity("OnlineStore.DataAccess.Models.Product", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Price")
                        .HasColumnType("float");

                    b.Property<Guid?>("PurchaseId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<Guid>("StoreId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("PurchaseId");

                    b.HasIndex("StoreId");

                    b.ToTable("Products");

                    b.HasData(
                        new
                        {
                            Id = new Guid("1109a562-aa69-4d01-8eba-10175eeaad5c"),
                            Description = "tasty tomatoes",
                            Name = "Tomato",
                            Price = 50.0,
                            Quantity = 10,
                            StoreId = new Guid("52fdf31c-27c4-498b-bad3-d56394b8d51d")
                        },
                        new
                        {
                            Id = new Guid("74757949-c32b-44ea-a7d0-0bf457b8a90e"),
                            Description = "Crisp, refreshing, and packed with subtle sweetness, cucumbers are one of nature’s most versatile and hydrating vegetables. With a vibrant green skin that encases pale, juicy flesh, cucumbers offer a mild yet distinct flavor that complements a wide variety of dishes. They are known for their high water content, making them an excellent ingredient for salads, sandwiches, pickles, and even smoothies.\r\n\r\nWhen freshly picked, cucumbers have a satisfying crunch that bursts with moisture upon each bite. Their cooling sensation makes them especially delightful in warm weather, providing a crisp contrast to rich or spicy foods. Some varieties, such as English cucumbers, have thin, nearly seedless skins that are tender and delicate, while others, like garden cucumbers, have a thicker, waxy exterior and more noticeable seeds, contributing to a heartier texture.\r\n\r\nBeyond their incredible taste and texture, cucumbers are also a nutritional powerhouse. They are rich in essential vitamins and minerals, including vitamin K, potassium, and antioxidants, while being naturally low in calories. Their hydrating properties help keep the body refreshed, making them an ideal snack for maintaining hydration throughout the day.\r\n\r\nIn the culinary world, cucumbers can be enjoyed raw, pickled, or blended into cooling soups like gazpacho. They pair beautifully with creamy dressings, tangy vinegars, and fragrant herbs such as dill, mint, and basil. When sliced thinly, cucumbers add a satisfying crispness to sandwiches, wraps, and sushi rolls. Meanwhile, their pickled form enhances flavors with a briny, tangy kick, making them a beloved addition to burgers, hot dogs, and charcuterie boards.\r\n\r\nWhether eaten straight from the garden, infused into refreshing drinks, or transformed into delicious pickles, cucumbers are a simple yet extraordinary ingredient that brings freshness and vibrancy to any dish. Their naturally mild flavor, delightful crunch, and cooling effect make them an enduring favorite for food lovers around the world.",
                            Name = "Cucumber",
                            Price = 30.0,
                            Quantity = 20,
                            StoreId = new Guid("52fdf31c-27c4-498b-bad3-d56394b8d51d")
                        },
                        new
                        {
                            Id = new Guid("b267e6ca-9576-4e31-aa21-850bb67ac236"),
                            Description = "sweet banana",
                            Name = "Banana",
                            Price = 50.0,
                            Quantity = 10,
                            StoreId = new Guid("3d5b47e7-d807-418a-856c-8b4dc0167787")
                        },
                        new
                        {
                            Id = new Guid("9c5a533b-8b81-4cba-9969-7e54020d0872"),
                            Description = "delicious cucumbers",
                            Name = "Cucumber",
                            Price = 35.0,
                            Quantity = 50,
                            StoreId = new Guid("3d5b47e7-d807-418a-856c-8b4dc0167787")
                        });
                });

            modelBuilder.Entity("OnlineStore.DataAccess.Models.ProductToCart", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CartId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ProductId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CartId");

                    b.HasIndex("ProductId");

                    b.ToTable("ProductToCarts");
                });

            modelBuilder.Entity("OnlineStore.DataAccess.Models.Purchase", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Purchases");
                });

            modelBuilder.Entity("OnlineStore.DataAccess.Models.Store", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Stores");

                    b.HasData(
                        new
                        {
                            Id = new Guid("52fdf31c-27c4-498b-bad3-d56394b8d51d"),
                            Name = "Walmart"
                        },
                        new
                        {
                            Id = new Guid("3d5b47e7-d807-418a-856c-8b4dc0167787"),
                            Name = "Lidl"
                        });
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<System.Guid>", b =>
                {
                    b.HasOne("OnlineStore.DataAccess.Models.ApplicationRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<System.Guid>", b =>
                {
                    b.HasOne("OnlineStore.DataAccess.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<System.Guid>", b =>
                {
                    b.HasOne("OnlineStore.DataAccess.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<System.Guid>", b =>
                {
                    b.HasOne("OnlineStore.DataAccess.Models.ApplicationRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("OnlineStore.DataAccess.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<System.Guid>", b =>
                {
                    b.HasOne("OnlineStore.DataAccess.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("OnlineStore.DataAccess.Models.Cart", b =>
                {
                    b.HasOne("OnlineStore.DataAccess.Models.ApplicationUser", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("OnlineStore.DataAccess.Models.Product", b =>
                {
                    b.HasOne("OnlineStore.DataAccess.Models.Purchase", "Purchase")
                        .WithMany("Products")
                        .HasForeignKey("PurchaseId");

                    b.HasOne("OnlineStore.DataAccess.Models.Store", "Store")
                        .WithMany("Products")
                        .HasForeignKey("StoreId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Purchase");

                    b.Navigation("Store");
                });

            modelBuilder.Entity("OnlineStore.DataAccess.Models.ProductToCart", b =>
                {
                    b.HasOne("OnlineStore.DataAccess.Models.Cart", "Cart")
                        .WithMany("ProductToCarts")
                        .HasForeignKey("CartId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("OnlineStore.DataAccess.Models.Product", "Product")
                        .WithMany("ProductToCarts")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Cart");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("OnlineStore.DataAccess.Models.Purchase", b =>
                {
                    b.HasOne("OnlineStore.DataAccess.Models.ApplicationUser", "User")
                        .WithMany("Purchases")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("OnlineStore.DataAccess.Models.ApplicationUser", b =>
                {
                    b.Navigation("Purchases");
                });

            modelBuilder.Entity("OnlineStore.DataAccess.Models.Cart", b =>
                {
                    b.Navigation("ProductToCarts");
                });

            modelBuilder.Entity("OnlineStore.DataAccess.Models.Product", b =>
                {
                    b.Navigation("ProductToCarts");
                });

            modelBuilder.Entity("OnlineStore.DataAccess.Models.Purchase", b =>
                {
                    b.Navigation("Products");
                });

            modelBuilder.Entity("OnlineStore.DataAccess.Models.Store", b =>
                {
                    b.Navigation("Products");
                });
#pragma warning restore 612, 618
        }
    }
}
