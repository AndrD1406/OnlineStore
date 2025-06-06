﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OnlineStore.DataAccess.Models;
using OnlineStore.DataAccess.Models.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineStore.DataAccess;

public class OnlineStoreDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
{
    public DbSet<Product> Products { get; set; }
    public DbSet<Purchase> Purchases { get; set; }
    public DbSet<Store> Stores { get; set; }
    public DbSet<Cart> Carts { get; set; }
    public DbSet<ProductToCart> ProductToCarts { get; set; }
    public DbSet<PurchaseItem> PurchaseItems { get; set; }
    public OnlineStoreDbContext(DbContextOptions options) : base(options)
    {

    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new ProductConfiguration());
        modelBuilder.ApplyConfiguration(new StoreConfiguration());
        modelBuilder.ApplyConfiguration(new PurchaseConfiguration());
        modelBuilder.ApplyConfiguration(new RoleConfiguration());
        modelBuilder.ApplyConfiguration(new CartConfiguration());
        modelBuilder.ApplyConfiguration(new ProductToCartConfiguration());
        modelBuilder.ApplyConfiguration(new PurchaseItemConfiguration());
        base.OnModelCreating(modelBuilder);
    }
}
