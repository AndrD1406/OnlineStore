using Bogus;
using OnlineStore.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineStore.Tests.TestDataGenerators;

public static class CartGenerator
{
    private static Faker<Cart> faker = new Faker<Cart>()
        .RuleFor(c => c.Id, _ => Guid.NewGuid())
        .RuleFor(c => c.UserId, _ => Guid.NewGuid())
        // By default, initialize with an empty list of ProductToCarts.
        .RuleFor(c => c.ProductToCarts, f => new List<ProductToCart>());

    public static Cart Generate() => faker.Generate();

    public static List<Cart> Generate(int count) => faker.Generate(count);

    public static Cart WithUser(this Cart cart, ApplicationUser user = null)
    {
        // If no user is provided, create a simple dummy ApplicationUser.
        user ??= new ApplicationUser { Id = Guid.NewGuid(), UserName = "TestUser" };
        cart.User = user;
        cart.UserId = user.Id;
        return cart;
    }

    public static Cart WithProducts(this Cart cart, int count = 3)
    {
        // Use the ProductToCartGenerator to generate a list of associations.
        var productToCarts = ProductToCartGenerator.Generate(count);
        foreach (var ptc in productToCarts)
        {
            ptc.Cart = cart;
            ptc.CartId = cart.Id;
        }
        cart.ProductToCarts = productToCarts;
        return cart;
    }
}