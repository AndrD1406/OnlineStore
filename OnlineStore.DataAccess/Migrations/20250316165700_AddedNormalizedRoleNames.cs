using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineStore.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddedNormalizedRoleNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("292ee265-a479-4a3b-a222-e97b45c6d1a1"),
                column: "NormalizedName",
                value: "USER");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("eafe291d-eca5-4c09-90fe-4faba2fa479d"),
                column: "NormalizedName",
                value: "ADMIN");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("292ee265-a479-4a3b-a222-e97b45c6d1a1"),
                column: "NormalizedName",
                value: null);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("eafe291d-eca5-4c09-90fe-4faba2fa479d"),
                column: "NormalizedName",
                value: null);
        }
    }
}
