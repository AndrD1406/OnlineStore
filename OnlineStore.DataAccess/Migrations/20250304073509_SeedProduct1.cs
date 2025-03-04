using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace OnlineStore.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class SeedProduct1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Description", "Name", "Price", "PurchaseId", "Quantity", "StoreId" },
                values: new object[,]
                {
                    { new Guid("1109a562-aa69-4d01-8eba-10175eeaad5c"), "tasty tomatoes", "Tomato", 50.0, new Guid("00000000-0000-0000-0000-000000000000"), 10, new Guid("52fdf31c-27c4-498b-bad3-d56394b8d51d") },
                    { new Guid("74757949-c32b-44ea-a7d0-0bf457b8a90e"), "tasty cucumbers", "Cucumber", 30.0, new Guid("00000000-0000-0000-0000-000000000000"), 20, new Guid("52fdf31c-27c4-498b-bad3-d56394b8d51d") }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("1109a562-aa69-4d01-8eba-10175eeaad5c"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("74757949-c32b-44ea-a7d0-0bf457b8a90e"));
        }
    }
}
