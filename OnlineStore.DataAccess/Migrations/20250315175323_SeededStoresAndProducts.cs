using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace OnlineStore.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class SeededStoresAndProducts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Stores",
                keyColumn: "Id",
                keyValue: new Guid("52fdf31c-27c4-498b-bad3-d56394b8d51d"),
                column: "Name",
                value: "Walmart");

            migrationBuilder.InsertData(
                table: "Stores",
                columns: new[] { "Id", "Name" },
                values: new object[] { new Guid("3d5b47e7-d807-418a-856c-8b4dc0167787"), "Lidl" });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Description", "Name", "Price", "PurchaseId", "Quantity", "StoreId" },
                values: new object[,]
                {
                    { new Guid("9c5a533b-8b81-4cba-9969-7e54020d0872"), "delicious cucumbers", "Cucumber", 35.0, null, 50, new Guid("3d5b47e7-d807-418a-856c-8b4dc0167787") },
                    { new Guid("b267e6ca-9576-4e31-aa21-850bb67ac236"), "sweet banana", "Banana", 50.0, null, 10, new Guid("3d5b47e7-d807-418a-856c-8b4dc0167787") }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("9c5a533b-8b81-4cba-9969-7e54020d0872"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("b267e6ca-9576-4e31-aa21-850bb67ac236"));

            migrationBuilder.DeleteData(
                table: "Stores",
                keyColumn: "Id",
                keyValue: new Guid("3d5b47e7-d807-418a-856c-8b4dc0167787"));

            migrationBuilder.UpdateData(
                table: "Stores",
                keyColumn: "Id",
                keyValue: new Guid("52fdf31c-27c4-498b-bad3-d56394b8d51d"),
                column: "Name",
                value: null);
        }
    }
}
