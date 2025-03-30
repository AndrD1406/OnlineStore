using Bogus;
using OnlineStore.DataAccess.Models;
using OnlineStore.Tests.Generators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineStore.Tests.TestDataGenerators;

public static class StoreGenerator
{
    private static Faker<Store> faker = new Faker<Store>()
        .RuleFor(s => s.Id, _ => Guid.NewGuid())
        .RuleFor(s => s.Name, f => f.Company.CompanyName())
        // Initialize with an empty list by default
        .RuleFor(s => s.Products, f => new List<Product>());

    public static Store Generate() => faker.Generate();

    public static List<Store> Generate(int count) => faker.Generate(count);

    public static Store WithProducts(this Store store, int count = 3)
    {
        // Generate 'count' number of products and assign them to the store
        store.Products = ProductGenerator.Generate(count);
        return store;
    }
}
