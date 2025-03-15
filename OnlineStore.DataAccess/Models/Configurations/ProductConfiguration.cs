using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineStore.DataAccess.Models.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasData(
            new Product() { Id = Guid.Parse("1109A562-AA69-4D01-8EBA-10175EEAAD5C"), Name = "Tomato", Description = "tasty tomatoes", Price = 50, Quantity = 10,  StoreId = Guid.Parse("52FDF31C-27C4-498B-BAD3-D56394B8D51D") },
            new Product() { Id = Guid.Parse("74757949-C32B-44EA-A7D0-0BF457B8A90E"), Name = "Cucumber", Description = "tasty cucumbers", Price = 30, Quantity = 20,  StoreId = Guid.Parse("52FDF31C-27C4-498B-BAD3-D56394B8D51D") },
            new Product() { Id = Guid.Parse("B267E6CA-9576-4E31-AA21-850BB67AC236"), Name = "Banana", Description = "sweet banana", Price = 50, Quantity = 10, StoreId = Guid.Parse("3D5B47E7-D807-418A-856C-8B4DC0167787") },
            new Product() { Id = Guid.Parse("9C5A533B-8B81-4CBA-9969-7E54020D0872"), Name = "Cucumber", Description = "delicious cucumbers", Price = 35, Quantity = 50, StoreId = Guid.Parse("3D5B47E7-D807-418A-856C-8B4DC0167787") }
            );
        }
    }
}
