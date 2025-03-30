using Bogus;
using OnlineStore.DataAccess.Models;
using OnlineStore.Tests.TestDataGenerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineStore.Tests.Generators;

public static class ProductGenerator
{
    private static Faker<Product> faker = new Faker<Product>()
        .RuleFor(p => p.Id, _ => Guid.NewGuid())
        .RuleFor(p => p.Name, f => f.Commerce.ProductName())
        .RuleFor(p => p.Description, f => f.Commerce.ProductDescription())
        .RuleFor(p => p.Quantity, f => f.Random.Number(1, 100))
        .RuleFor(p => p.Price, f => Math.Round(f.Random.Double(1, 1000), 2))
        // By default, generate a random StoreId; use WithStore to override with a proper store.
        .RuleFor(p => p.StoreId, _ => Guid.NewGuid())
        // By default, no purchase is assigned.
        .RuleFor(p => p.PurchaseId, _ => (Guid?)null)
        .RuleFor(p => p.ProductToCarts, f => new List<ProductToCart>());

    public static Product Generate() => faker.Generate();

    public static List<Product> Generate(int count) => faker.Generate(count);

    public static Product WithId(this Product product, Guid id)
    {
        product.Id = id;
        return product;
    }

    public static Product WithStore(this Product product, Store store = null)
    {
        // Assume you have a StoreGenerator; if not, create a simple one.
        store ??= StoreGenerator.Generate();
        product.Store = store;
        product.StoreId = store.Id;
        return product;
    }

    public static Product WithPurchase(this Product product, Purchase purchase = null)
    {
        if (purchase != null)
        {
            product.Purchase = purchase;
            product.PurchaseId = purchase.Id;
        }
        return product;
    }
}
