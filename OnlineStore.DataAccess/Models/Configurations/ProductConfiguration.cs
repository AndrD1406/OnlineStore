using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineStore.DataAccess.Models.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasData(
        new Product() { Id = Guid.Parse("1109A562-AA69-4D01-8EBA-10175EEAAD5C"), Name = "Tomato", Description = "tasty tomatoes", Price = 50, Quantity = 10,  StoreId = Guid.Parse("52FDF31C-27C4-498B-BAD3-D56394B8D51D") },
        new Product() { Id = Guid.Parse("74757949-C32B-44EA-A7D0-0BF457B8A90E"), Name = "Cucumber", Description = "Crisp, refreshing, and packed with subtle sweetness, cucumbers are one of nature’s most versatile and hydrating vegetables. With a vibrant green skin that encases pale, juicy flesh, cucumbers offer a mild yet distinct flavor that complements a wide variety of dishes. They are known for their high water content, making them an excellent ingredient for salads, sandwiches, pickles, and even smoothies.\r\n\r\nWhen freshly picked, cucumbers have a satisfying crunch that bursts with moisture upon each bite. Their cooling sensation makes them especially delightful in warm weather, providing a crisp contrast to rich or spicy foods. Some varieties, such as English cucumbers, have thin, nearly seedless skins that are tender and delicate, while others, like garden cucumbers, have a thicker, waxy exterior and more noticeable seeds, contributing to a heartier texture.\r\n\r\nBeyond their incredible taste and texture, cucumbers are also a nutritional powerhouse. They are rich in essential vitamins and minerals, including vitamin K, potassium, and antioxidants, while being naturally low in calories. Their hydrating properties help keep the body refreshed, making them an ideal snack for maintaining hydration throughout the day.\r\n\r\nIn the culinary world, cucumbers can be enjoyed raw, pickled, or blended into cooling soups like gazpacho. They pair beautifully with creamy dressings, tangy vinegars, and fragrant herbs such as dill, mint, and basil. When sliced thinly, cucumbers add a satisfying crispness to sandwiches, wraps, and sushi rolls. Meanwhile, their pickled form enhances flavors with a briny, tangy kick, making them a beloved addition to burgers, hot dogs, and charcuterie boards.\r\n\r\nWhether eaten straight from the garden, infused into refreshing drinks, or transformed into delicious pickles, cucumbers are a simple yet extraordinary ingredient that brings freshness and vibrancy to any dish. Their naturally mild flavor, delightful crunch, and cooling effect make them an enduring favorite for food lovers around the world.", Price = 30, Quantity = 20,  StoreId = Guid.Parse("52FDF31C-27C4-498B-BAD3-D56394B8D51D") },
        new Product() { Id = Guid.Parse("B267E6CA-9576-4E31-AA21-850BB67AC236"), Name = "Banana", Description = "sweet banana", Price = 50, Quantity = 10, StoreId = Guid.Parse("3D5B47E7-D807-418A-856C-8B4DC0167787") },
        new Product() { Id = Guid.Parse("9C5A533B-8B81-4CBA-9969-7E54020D0872"), Name = "Cucumber", Description = "delicious cucumbers", Price = 35, Quantity = 50, StoreId = Guid.Parse("3D5B47E7-D807-418A-856C-8B4DC0167787") }
        );
    }
}
