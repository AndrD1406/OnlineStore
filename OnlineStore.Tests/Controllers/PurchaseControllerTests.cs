using Bogus;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using OnlineStore.BusinessLogic.Dtos;
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
public class PurchaseControllerTests
{
    private Mock<IPurchaseService> _purchaseService;
    private Mock<ICartService> _cartService;
    private Mock<IProductService> _productService;
    private PurchaseController _purchaseController;
    private Faker faker;

    [SetUp]
    public void Setup()
    {
        _purchaseService = new Mock<IPurchaseService>();
        _cartService = new Mock<ICartService>();
        _productService = new Mock<IProductService>();
        _purchaseController = new PurchaseController(_purchaseService.Object, _cartService.Object, _productService.Object);
        faker = new Faker();
    }
    [TearDown]
    public void TearDown()
    {
        _purchaseController.Dispose();
    }
    [Test]
    public async Task Index_WhenAuthorized_ReturnsView()
    {
        // Arrange
        var userId = Guid.NewGuid().ToString();
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

        _purchaseController.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = new ClaimsPrincipal(identity) }
        };

        // Act
        var result = await _purchaseController.Index();

        // Assert
        var viewResult = result as ViewResult;
        viewResult.Should().NotBeNull();
    }
    [Test]
    public async Task Create_WhenCartIsEmpty_ReturnsView()
    {
        // Arrange
        var userId = Guid.NewGuid().ToString();
        var userIdParsed = Guid.Parse(userId);
        var user = new ApplicationUser
        {
            Id = userIdParsed,
            Name = "Test User",
            Email = "test@example.com",
            PhoneNumber = "1234567890"
        };
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, userId)
        };
        var identity = new ClaimsIdentity(claims);

        _purchaseController.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = new ClaimsPrincipal(identity) }
        };

        var tempData = new Mock<ITempDataDictionary>();
        _purchaseController.TempData = tempData.Object;

        _cartService.Setup(x => x.GetByUserId(userIdParsed)).ReturnsAsync(new List<Cart>());

        // Act
        var result = await _purchaseController.Create();

        // Assert
        var viewResult = result as RedirectToActionResult;
        viewResult.Should().NotBeNull();
        viewResult.ActionName.Should().Be("Index");
        viewResult.ControllerName.Should().Be("Cart");
    }
}
