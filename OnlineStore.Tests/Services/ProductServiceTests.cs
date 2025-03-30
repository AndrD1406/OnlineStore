using Bogus;
using FluentAssertions;
using Moq;
using OnlineStore.BusinessLogic.Services;
using OnlineStore.BusinessLogic.Services.Interfaces;
using OnlineStore.DataAccess.Models;
using OnlineStore.DataAccess.Repository.Base;
using OnlineStore.Tests.Generators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace OnlineStore.Tests.Services;

[TestFixture]
public class ProductServiceTests
{
    private Mock<IEntityRepository<Guid, Product>> _repositoryMock;
    private IProductService _productService;

    [SetUp]
    public void Setup()
    {
        _repositoryMock = new Mock<IEntityRepository<Guid, Product>>();
        _productService = new ProductService(_repositoryMock.Object);
    }

    [Test]
    public async Task GetAll_WhenCalled_ReturnsAllProducts()
    {
        // Arrange
        var products = ProductGenerator.Generate(5);
        _repositoryMock.Setup(r => r.GetAll()).ReturnsAsync(products);

        // Act
        var result = await _productService.GetAll();

        // Assert
        result.Should().BeEquivalentTo(products);
        _repositoryMock.Verify(r => r.GetAll(), Times.Once);
    }

    [Test]
    public async Task Create_WhenCalled_ReturnsCreatedProduct()
    {
        // Arrange
        var product = ProductGenerator.Generate();
        _repositoryMock.Setup(r => r.Create(It.IsAny<Product>())).ReturnsAsync(product);

        // Act
        var result = await _productService.Create(product);

        // Assert
        result.Should().BeEquivalentTo(product);
        _repositoryMock.Verify(r => r.Create(It.IsAny<Product>()), Times.Once);
    }

    [Test]
    public async Task GetByStore_WhenCalled_ReturnsProductsForStore()
    {
        // Arrange
        var storeId = Guid.NewGuid();
        var products = ProductGenerator.Generate(3)
            .Select(p => { p.StoreId = storeId; return p; })
            .ToList();
        _repositoryMock
            .Setup(r => r.GetByFilter(It.IsAny<Expression<Func<Product, bool>>>(), It.IsAny<string>()))
            .ReturnsAsync(products);

        // Act
        var result = await _productService.GetByStore(storeId);

        // Assert
        result.Should().BeEquivalentTo(products);
        _repositoryMock.Verify(
            r => r.GetByFilter(It.IsAny<Expression<Func<Product, bool>>>(), It.IsAny<string>()),
            Times.Once);
    }

    [Test]
    public async Task Filter_WhenCalled_ReturnsFilteredProducts()
    {
        // Arrange
        var products = ProductGenerator.Generate(5);
        Expression<Func<Product, bool>> filterExpression = p => p.Price > 100;
        var filteredProducts = products.Where(filterExpression.Compile()).ToList();
        _repositoryMock
            .Setup(r => r.GetByFilter(It.IsAny<Expression<Func<Product, bool>>>(), It.IsAny<string>()))
            .ReturnsAsync(filteredProducts);

        // Act
        var result = await _productService.Filter(filterExpression);

        // Assert
        result.Should().BeEquivalentTo(filteredProducts);
        _repositoryMock.Verify(
            r => r.GetByFilter(It.IsAny<Expression<Func<Product, bool>>>(), It.IsAny<string>()),
            Times.Once);
    }

    [Test]
    public async Task UpdateProduct_WhenCalled_UpdatesProduct()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var originalProduct = ProductGenerator.Generate().WithId(productId);
        var updatedProduct = ProductGenerator.Generate().WithId(productId);
        updatedProduct.Name = "Updated Name";
        updatedProduct.Description = "Updated Description";
        updatedProduct.Price = originalProduct.Price + 10;
        updatedProduct.Quantity = originalProduct.Quantity - 1;

        _repositoryMock.Setup(r => r.GetById(productId)).ReturnsAsync(originalProduct);
        _repositoryMock.Setup(r => r.Update(It.IsAny<Product>())).ReturnsAsync(updatedProduct);

        // Act
        var result = await _productService.UpdateProduct(productId, updatedProduct);

        // Assert
        result.Should().BeEquivalentTo(updatedProduct);
        _repositoryMock.Verify(r => r.GetById(productId), Times.Once);
        _repositoryMock.Verify(r => r.Update(It.IsAny<Product>()), Times.Once);
    }
}
