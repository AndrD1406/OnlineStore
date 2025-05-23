﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineStore.DataAccess.Models.Configurations;

public class StoreConfiguration : IEntityTypeConfiguration<Store>
{
    public void Configure(EntityTypeBuilder<Store> builder)
    {
        builder.HasData(new Store() { Id = Guid.Parse("52FDF31C-27C4-498B-BAD3-D56394B8D51D"), Name="Walmart" }, new Store() { Id = Guid.Parse("3D5B47E7-D807-418A-856C-8B4DC0167787"), Name = "Lidl" });
    }
}
