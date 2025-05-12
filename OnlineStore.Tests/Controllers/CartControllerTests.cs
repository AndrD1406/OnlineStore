using Bogus;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using OnlineStore.BusinessLogic.Services.Interfaces;
using OnlineStore.Controllers;
using OnlineStore.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace OnlineStore.Tests.Controllers;

public class CartControllerTests
{
    private Mock<ICartService> _cartService;
    private CartController _cartController;
    private Faker faker;

    [SetUp]
    public void Setup()
    {
        _cartService = new Mock<ICartService>();
        _cartController = new CartController(_cartService.Object);
        faker = new Faker();
    }
    [TearDown]
    public void TearDown()
    {
        _cartController.Dispose();
    }
    [Test]
    public async Task Index_AuthorizedWhenNoCartExists_ReturnsViewCart()
    {
        // Arrange
        var userId = Guid.NewGuid().ToString();
        var userIdParsed = Guid.Parse(userId);
        var user = new ApplicationUser
        {
            Id = Guid.Parse(userId),
            Name = "Test User",
            Email = "test@example.com",
            PhoneNumber = "1234567890"
        };
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, userId)
        };
        var identity = new ClaimsIdentity(claims);

        _cartController.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = new ClaimsPrincipal(identity) }
        };

        _cartService.Setup(x => x.GetByUserId(userIdParsed)).ReturnsAsync(new List<Cart>());

        // Act
        var result = await _cartController.Index();

        // Assert
        var viewResult = (ViewResult)result;
        viewResult.Model.Should().Be(null as Cart);
    }
    [Test]
    public async Task Add_AuthorizedWhenValidProduct_ReturnsViewCart()
    {
        // Arrange
        var userId = Guid.NewGuid().ToString();
        var userIdParsed = Guid.Parse(userId);
        var user = new ApplicationUser
        {
            Id = Guid.Parse(userId),
            Name = "Test User",
            Email = "test@example.com",
            PhoneNumber = "1234567890"
        };
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, userId)
        };
        var identity = new ClaimsIdentity(claims);

        _cartController.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = new ClaimsPrincipal(identity) }
        };

        Cart cart = new Cart() { Id = Guid.NewGuid(), UserId = userIdParsed };

        _cartService.Setup(x => x.GetByUserId(userIdParsed)).ReturnsAsync(new List<Cart>() { cart });
        Guid productId = Guid.NewGuid();
        int quantity = 10;

        // Act
        var result = await _cartController.Add(productId, quantity);

        // Assert
        var viewResult = (RedirectToActionResult)result;
        viewResult.ActionName.Should().Be("Index");
    }

    [Test]
    public async Task Remove_AuthorizedWhenProductExists_ReturnsViewCart()
    {
        // Arrange
        var userId = Guid.NewGuid().ToString();
        var userIdParsed = Guid.Parse(userId);
        var user = new ApplicationUser
        {
            Id = Guid.Parse(userId),
            Name = "Test User",
            Email = "test@example.com",
            PhoneNumber = "1234567890"
        };
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, userId)
        };
        var identity = new ClaimsIdentity(claims);

        _cartController.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = new ClaimsPrincipal(identity) }
        };

        Cart cart = new Cart() { Id = Guid.NewGuid(), UserId = userIdParsed };

        _cartService.Setup(x => x.GetByUserId(userIdParsed)).ReturnsAsync(new List<Cart>() { cart });
        Guid productId = Guid.NewGuid();

        // Act
        var result = await _cartController.Remove(productId);

        // Assert
        var viewResult = (RedirectToActionResult)result;
        viewResult.ActionName.Should().Be("Index");
    }
}

