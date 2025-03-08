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
            builder.HasData(new List<Product> { 
            new Product(){Id = Guid.Parse("1109A562-AA69-4D01-8EBA-10175EEAAD5C"), Name="Tomato", Description="tasty tomatoes", Price=50, Quantity=10, PurchaseId=Guid.Parse("DF44D370-E174-4426-A52E-58A260F704EB"), StoreId=Guid.Parse("52FDF31C-27C4-498B-BAD3-D56394B8D51D")},
            new Product(){Id = Guid.Parse("74757949-C32B-44EA-A7D0-0BF457B8A90E"), Name="Cucumber", Description="tasty cucumbers", Price=30, Quantity=20, PurchaseId=Guid.Parse("DF44D370-E174-4426-A52E-58A260F704EB"), StoreId=Guid.Parse("52FDF31C-27C4-498B-BAD3-D56394B8D51D")},
            });
        }
    }
}
