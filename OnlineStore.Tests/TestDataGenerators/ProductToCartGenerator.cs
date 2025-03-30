using Bogus;
using OnlineStore.DataAccess.Models;
using OnlineStore.Tests.Generators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineStore.Tests.TestDataGenerators;

public static class ProductToCartGenerator
{
    private static Faker<ProductToCart> faker = new Faker<ProductToCart>()
        .RuleFor(ptc => ptc.Id, _ => Guid.NewGuid())
        .RuleFor(ptc => ptc.ProductId, _ => Guid.NewGuid())
        .RuleFor(ptc => ptc.CartId, _ => Guid.NewGuid())
        .RuleFor(ptc => ptc.Quantity, f => f.Random.Number(1, 5))
        // Leave navigation properties null by default; use WithProduct/WithCart to assign them.
        .RuleFor(ptc => ptc.Product, _ => null)
        .RuleFor(ptc => ptc.Cart, _ => null);

    public static ProductToCart Generate() => faker.Generate();

    public static List<ProductToCart> Generate(int count) => faker.Generate(count);

    public static ProductToCart WithProduct(this ProductToCart ptc, Product product = null)
    {
        product ??= ProductGenerator.Generate();
        ptc.Product = product;
        ptc.ProductId = product.Id;
        return ptc;
    }

    public static ProductToCart WithCart(this ProductToCart ptc, Cart cart = null)
    {
        cart ??= CartGenerator.Generate();
        ptc.Cart = cart;
        ptc.CartId = cart.Id;
        return ptc;
    }
}