using Bogus;
using Castle.Core.Logging;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using OnlineStore.BusinessLogic.Services;
using OnlineStore.BusinessLogic.Services.Interfaces;
using OnlineStore.Controllers;
using OnlineStore.DataAccess.Models;
using OnlineStore.DataAccess.Repository.Base;
using OnlineStore.Tests.Generators;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace OnlineStore.Tests.Controllers;
public class ProductControllerTests
{
    private Mock<IProductService> productServiceMock;
    private Mock<IStoreService> storeServiceMock;
    private Mock<IPurchaseService> purchaseServiceMock;
    private ProductController productController;
    private Faker faker;

    [SetUp]
    public void Setup()
    {
        productServiceMock = new Mock<IProductService>();
        storeServiceMock = new Mock<IStoreService>();
        purchaseServiceMock = new Mock<IPurchaseService>();

        productController = new ProductController(
            productServiceMock.Object,
            storeServiceMock.Object,
            purchaseServiceMock.Object
        );

        faker = new Faker();
    }

    [TearDown]
    public void TearDown()
    {
        productController.Dispose();
    }

    [Test]
    public async Task Index_WhenNoFilter_ReturnsAllProducts()
    {
        // Arrange
        var products = ProductGenerator.Generate(5);
        productServiceMock.Setup(s => s.Filter(It.IsAny<Expression<Func<Product, bool>>>()))
            .ReturnsAsync(products);
        productServiceMock.Setup(s => s.GetAll()).ReturnsAsync(products);

        // Act
        var result = await productController.Index(null, null, null, null);
        var viewResult = (ViewResult)result;

        // Assert
        viewResult.Model.Should().BeEquivalentTo(products);
    }
    [Test]
    public async Task Index_WhenFilter_ReturnsFilteredProducts()
    {
        // Arrange
        var products = ProductGenerator.Generate(5);
        var price = faker.Random.Int(1, 1000);
        Expression<Func<Product, bool>> filterExpression = p => p.Price <= price;
        var filteredProducts = products.Where(filterExpression.Compile()).ToList();
        productServiceMock.Setup(s => s.Filter(It.IsAny<Expression<Func<Product, bool>>>())).ReturnsAsync(filteredProducts);
        productServiceMock.Setup(s => s.GetAll()).ReturnsAsync(products);

        // Act
        var result = await productController.Index(null, null, 0, price);
        var viewResult = (ViewResult)result;

        // Assert
        viewResult.Model.Should().BeEquivalentTo(filteredProducts);
    }
    [Test]
    public async Task GetProductsByStore_WhenStoreExist_ReturnsProductsForStore()
    {
        // Arrange
        var storeId = Guid.NewGuid();
        var filteredProducts = ProductGenerator.Generate(10)
            .Select((p, i) => { if (i % 2 == 0) p.StoreId = storeId; return p; })
            .ToList();
        productServiceMock
            .Setup(s => s.GetByStore(storeId))
            .ReturnsAsync(filteredProducts);

        // Act
        var result = await productController.GetProductsByStore(storeId);
        var viewResult = (ViewResult)result;

        // Assert
        viewResult.Model.Should().BeEquivalentTo(filteredProducts);
        productServiceMock.Verify(
            s =>s.GetByStore(storeId),
            Times.Once);
    }
    [Test]
    public async Task GetProductsByStore_WhenStoreDoesntExist_ReturnsEmptyList()
    {
        // Arrange
        var storeId = Guid.NewGuid();
        var products = ProductGenerator.Generate(10)
            .ToList();
        productServiceMock
            .Setup(s => s.GetByStore(storeId))
            .ReturnsAsync(new List<Product>());
        List<Product> emptyList = [];

        // Act
        var result = await productController.GetProductsByStore(storeId);
        var viewResult = (ViewResult)result;

        // Assert
        viewResult.Model.Should().BeEquivalentTo(emptyList);
        productServiceMock.Verify(
            s => s.GetByStore(storeId),
            Times.Once);
    }
    [Test]
    public async Task Details_WhenProductExist_ReturnsProduct()
    {
        // Arrange
        var products = ProductGenerator.Generate(10);
        var product = products[5];
        var productId = product.Id;
        var filteredProducts = products.Where(x => x.Id == productId);
        productServiceMock.Setup(s => s.Filter(It.IsAny<Expression<Func<Product, bool>>>())).ReturnsAsync(filteredProducts);

        // Act
        var result = await productController.Details(productId);
        var viewResult = (ViewResult)result;

        // Assert
        viewResult.Model.Should().Be(product);
        productServiceMock.Verify(s => s.Filter(It.IsAny<Expression<Func<Product, bool>>>()), Times.Once);
    }
    [Test]
    public async Task Details_WhenProductDoesntExist_ReturnsProduct()
    {
        // Arrange
        var products = ProductGenerator.Generate(10);
        var product = products[5];
        var productId = Guid.NewGuid();
        productServiceMock.Setup(s => s.Filter(It.IsAny<Expression<Func<Product, bool>>>())).ReturnsAsync(new List<Product>());

        // Act
        var result = await productController.Details(productId);
        var viewResult = (ViewResult)result;

        // Assert
        viewResult.Model.Should().Be(null);
        productServiceMock.Verify(s => s.Filter(It.IsAny<Expression<Func<Product, bool>>>()), Times.Once);
    }
}
