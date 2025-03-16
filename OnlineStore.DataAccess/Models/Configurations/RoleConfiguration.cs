using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineStore.DataAccess.Models.Configurations;
public class RoleConfiguration : IEntityTypeConfiguration<ApplicationRole>
{
    public void Configure(EntityTypeBuilder<ApplicationRole> builder)
    {
        builder.HasData(
            new ApplicationRole() { Id = Guid.Parse("292EE265-A479-4A3B-A222-E97B45C6D1A1"), Name = "User", NormalizedName="USER" },
            new ApplicationRole() { Id = Guid.Parse("EAFE291D-ECA5-4C09-90FE-4FABA2FA479D"), Name = "Admin", NormalizedName="ADMIN" }
        );
    }
}

