using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineStore.DataAccess.Models.Configurations
{
    public class PurchaseConfiguration : IEntityTypeConfiguration<Purchase>
    {
        public void Configure(EntityTypeBuilder<Purchase> builder)
        {
            //builder.HasData(new Purchase() { Id = Guid.Parse("DF44D370-E174-4426-A52E-58A260F704EB"), UserId = Guid.Parse("20da6577-ab76-47fc-a95a-22797094634c") });
        }
    }
}
