using Bogus;
using OnlineStore.DataAccess.Models;
using OnlineStore.Tests.Generators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineStore.Tests.TestDataGenerators;

public static class PurchaseGenerator
{
    private static Faker<Purchase> faker = new Faker<Purchase>()
        .RuleFor(p => p.Id, _ => Guid.NewGuid())
        .RuleFor(p => p.UserId, _ => Guid.NewGuid())
        .RuleFor(p => p.User, f => null) // No default user; use WithUser() to assign one.
        .RuleFor(p => p.CreatedDate, f => DateTime.Now)
        .RuleFor(p => p.TotalAmount, f => f.Random.Double(1, 1000))
        .RuleFor(p => p.Products, f => new List<Product>());

    public static Purchase Generate() => faker.Generate();

    public static List<Purchase> Generate(int count) => faker.Generate(count);

    public static Purchase WithUser(this Purchase purchase, ApplicationUser user = null)
    {
        user ??= new ApplicationUser { Id = Guid.NewGuid(), UserName = "TestUser" };
        purchase.User = user;
        purchase.UserId = user.Id;
        return purchase;
    }

    public static Purchase WithItems(this Purchase purchase, int count = 3)
    {
        purchase.PurchaseItems.Clear();

        var products = ProductGenerator.Generate(count);

        foreach (var prod in products)
        {
            var item = new PurchaseItem
            {
                Id = Guid.NewGuid(),
                PurchaseId = purchase.Id,
                ProductId = prod.Id,
                Product = prod,
                Quantity = 1,
                UnitPrice = prod.Price
            };
            purchase.PurchaseItems.Add(item);
        }

        purchase.TotalAmount = purchase.PurchaseItems
            .Sum(pi => pi.Quantity * pi.UnitPrice);

        return purchase;
    }

    public static Purchase WithTotalAmount(this Purchase purchase, double totalAmount)
    {
        purchase.TotalAmount = totalAmount;
        return purchase;
    }

    public static Purchase WithCreatedDate(this Purchase purchase, DateTime createdDate)
    {
        purchase.CreatedDate = createdDate;
        return purchase;
    }
}
