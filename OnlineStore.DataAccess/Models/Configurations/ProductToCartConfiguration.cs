using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineStore.DataAccess.Models.Configurations;

public class ProductToCartConfiguration : IEntityTypeConfiguration<ProductToCart>
{
    public void Configure(EntityTypeBuilder<ProductToCart> builder)
    {
        
    }
}
