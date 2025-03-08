using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineStore.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class SeedPurchase1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("1109a562-aa69-4d01-8eba-10175eeaad5c"),
                column: "PurchaseId",
                value: new Guid("df44d370-e174-4426-a52e-58a260f704eb"));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("74757949-c32b-44ea-a7d0-0bf457b8a90e"),
                column: "PurchaseId",
                value: new Guid("df44d370-e174-4426-a52e-58a260f704eb"));

            migrationBuilder.InsertData(
                table: "Purchases",
                columns: new[] { "Id", "UserId" },
                values: new object[] { new Guid("df44d370-e174-4426-a52e-58a260f704eb"), new Guid("20da6577-ab76-47fc-a95a-22797094634c") });

            migrationBuilder.InsertData(
                table: "Stores",
                columns: new[] { "Id", "Name" },
                values: new object[] { new Guid("52fdf31c-27c4-498b-bad3-d56394b8d51d"), null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Purchases",
                keyColumn: "Id",
                keyValue: new Guid("df44d370-e174-4426-a52e-58a260f704eb"));

            migrationBuilder.DeleteData(
                table: "Stores",
                keyColumn: "Id",
                keyValue: new Guid("52fdf31c-27c4-498b-bad3-d56394b8d51d"));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("1109a562-aa69-4d01-8eba-10175eeaad5c"),
                column: "PurchaseId",
                value: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("74757949-c32b-44ea-a7d0-0bf457b8a90e"),
                column: "PurchaseId",
                value: new Guid("00000000-0000-0000-0000-000000000000"));
        }
    }
}
