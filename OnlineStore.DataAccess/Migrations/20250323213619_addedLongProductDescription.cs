using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineStore.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class addedLongProductDescription : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("74757949-c32b-44ea-a7d0-0bf457b8a90e"),
                column: "Description",
                value: "Crisp, refreshing, and packed with subtle sweetness, cucumbers are one of nature’s most versatile and hydrating vegetables. With a vibrant green skin that encases pale, juicy flesh, cucumbers offer a mild yet distinct flavor that complements a wide variety of dishes. They are known for their high water content, making them an excellent ingredient for salads, sandwiches, pickles, and even smoothies.\r\n\r\nWhen freshly picked, cucumbers have a satisfying crunch that bursts with moisture upon each bite. Their cooling sensation makes them especially delightful in warm weather, providing a crisp contrast to rich or spicy foods. Some varieties, such as English cucumbers, have thin, nearly seedless skins that are tender and delicate, while others, like garden cucumbers, have a thicker, waxy exterior and more noticeable seeds, contributing to a heartier texture.\r\n\r\nBeyond their incredible taste and texture, cucumbers are also a nutritional powerhouse. They are rich in essential vitamins and minerals, including vitamin K, potassium, and antioxidants, while being naturally low in calories. Their hydrating properties help keep the body refreshed, making them an ideal snack for maintaining hydration throughout the day.\r\n\r\nIn the culinary world, cucumbers can be enjoyed raw, pickled, or blended into cooling soups like gazpacho. They pair beautifully with creamy dressings, tangy vinegars, and fragrant herbs such as dill, mint, and basil. When sliced thinly, cucumbers add a satisfying crispness to sandwiches, wraps, and sushi rolls. Meanwhile, their pickled form enhances flavors with a briny, tangy kick, making them a beloved addition to burgers, hot dogs, and charcuterie boards.\r\n\r\nWhether eaten straight from the garden, infused into refreshing drinks, or transformed into delicious pickles, cucumbers are a simple yet extraordinary ingredient that brings freshness and vibrancy to any dish. Their naturally mild flavor, delightful crunch, and cooling effect make them an enduring favorite for food lovers around the world.");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("74757949-c32b-44ea-a7d0-0bf457b8a90e"),
                column: "Description",
                value: "tasty cucumbers");
        }
    }
}
