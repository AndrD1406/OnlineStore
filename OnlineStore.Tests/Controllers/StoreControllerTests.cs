using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using OnlineStore.Controllers;
using OnlineStore.BusinessLogic.Services.Interfaces;
using OnlineStore.DataAccess.Models;
using OnlineStore.Models;
using OnlineStore.Tests.TestDataGenerators;
using OnlineStore.Tests.Generators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Bogus;

namespace OnlineStore.Tests.Controllers;

public class StoreControllerTests
{
    private Mock<IProductService> productServiceMock;
    private Mock<IStoreService> storeServiceMock;
    private StoreController storeController;
    private Faker faker;

    [SetUp]
    public void Setup()
    {
        productServiceMock = new Mock<IProductService>();
        storeServiceMock = new Mock<IStoreService>();
        storeController = new StoreController(productServiceMock.Object, storeServiceMock.Object);
        faker = new Faker();
    }

    [TearDown]
    public void TearDown()
    {
        storeController?.Dispose();
    }

    //[Test]
    //public async Task Index_ReturnsAllStores()
    //{
    //    // Arrange
    //    var stores = StoreGenerator.Generate(3);
    //    storeServiceMock.Setup(s => s.GetAll()).ReturnsAsync(stores);

    //    // Act
    //    var result = await storeController.Index();
    //    var viewResult = result as ViewResult;

    //    // Assert
    //    viewResult.Should().NotBeNull();
    //    viewResult!.Model.Should().BeEquivalentTo(stores);
    //}

    //[Test]
    //public async Task Details_WhenStoreExists_ReturnsViewWithProducts()
    //{
    //    // Arrange
    //    var store = StoreGenerator.Generate();
    //    var products = ProductGenerator.Generate(5).Select(p => p.WithStore(store)).ToList();

    //    storeServiceMock.Setup(s => s.Get(store.Id)).ReturnsAsync(store);
    //    productServiceMock.Setup(s => s.Filter(It.IsAny<Expression<Func<Product, bool>>>(), -1, -1)).ReturnsAsync(products);

    //    // Act
    //    var result = await storeController.Details(store.Id, null, null);
    //    var viewResult = result as ViewResult;
    //    var model = viewResult?.Model as StoreDetailsViewModel;

    //    // Assert
    //    viewResult.Should().NotBeNull();
    //    model.Should().NotBeNull();
    //    model!.Store.Should().Be(store);
    //    model.Products.Should().BeEquivalentTo(products);
    //}

    [Test]
    public async Task Details_WhenStoreNotFound_ReturnsNotFound()
    {
        // Arrange
        var storeId = Guid.NewGuid();
        storeServiceMock.Setup(s => s.Get(storeId)).ReturnsAsync((Store)null!);

        // Act
        var result = await storeController.Details(storeId, null, null);

        // Assert
        result.Should().BeOfType<NotFoundResult>();
    }

    [Test]
    public void Create_Get_ReturnsEmptyStoreView()
    {
        // Act
        var result = storeController.Create();
        var viewResult = result as ViewResult;

        // Assert
        viewResult.Should().NotBeNull();
        viewResult!.Model.Should().BeOfType<Store>();
    }

    [Test]
    public async Task Create_Post_ValidModel_RedirectsToIndex()
    {
        // Arrange
        var store = StoreGenerator.Generate();

        // Act
        var result = await storeController.Create(store);
        var redirectResult = result as RedirectToActionResult;

        // Assert
        storeServiceMock.Verify(s => s.Create(store), Times.Once);
        redirectResult.Should().NotBeNull();
        redirectResult!.ActionName.Should().Be(nameof(StoreController.Index));
    }

    [Test]
    public async Task Create_Post_InvalidModel_ReturnsSameView()
    {
        // Arrange
        var store = StoreGenerator.Generate();
        storeController.ModelState.AddModelError("Name", "Required");

        // Act
        var result = await storeController.Create(store);
        var viewResult = result as ViewResult;

        // Assert
        viewResult.Should().NotBeNull();
        viewResult!.Model.Should().Be(store);
    }

    [Test]
    public async Task Edit_Get_WhenStoreExists_ReturnsView()
    {
        // Arrange
        var store = StoreGenerator.Generate();
        storeServiceMock.Setup(s => s.Get(store.Id)).ReturnsAsync(store);

        // Act
        var result = await storeController.Edit(store.Id);
        var viewResult = result as ViewResult;

        // Assert
        viewResult.Should().NotBeNull();
        viewResult!.Model.Should().Be(store);
    }

    [Test]
    public async Task Edit_Get_WhenStoreNotFound_ReturnsNotFound()
    {
        // Arrange
        var id = Guid.NewGuid();
        storeServiceMock.Setup(s => s.Get(id)).ReturnsAsync((Store)null!);

        // Act
        var result = await storeController.Edit(id);

        // Assert
        result.Should().BeOfType<NotFoundResult>();
    }

    [Test]
    public async Task Edit_Post_ValidModel_UpdatesStoreAndRedirects()
    {
        // Arrange
        var store = StoreGenerator.Generate();

        // Act
        var result = await storeController.Edit(store.Id, store);
        var redirectResult = result as RedirectToActionResult;

        // Assert
        storeServiceMock.Verify(s => s.Update(store.Id, store), Times.Once);
        redirectResult.Should().NotBeNull();
        redirectResult!.ActionName.Should().Be(nameof(StoreController.Index));
    }

    [Test]
    public async Task Edit_Post_InvalidModel_ReturnsSameView()
    {
        // Arrange
        var store = StoreGenerator.Generate();
        storeController.ModelState.AddModelError("Name", "Required");

        // Act
        var result = await storeController.Edit(store.Id, store);
        var viewResult = result as ViewResult;

        // Assert
        viewResult.Should().NotBeNull();
        viewResult!.Model.Should().Be(store);
    }

    [Test]
    public async Task Delete_Get_WhenStoreExists_ReturnsView()
    {
        // Arrange
        var store = StoreGenerator.Generate();
        storeServiceMock.Setup(s => s.Get(store.Id)).ReturnsAsync(store);

        // Act
        var result = await storeController.Delete(store.Id);
        var viewResult = result as ViewResult;

        // Assert
        viewResult.Should().NotBeNull();
        viewResult!.Model.Should().Be(store);
    }

    [Test]
    public async Task Delete_Get_WhenStoreNotFound_ReturnsNotFound()
    {
        // Arrange
        var id = Guid.NewGuid();
        storeServiceMock.Setup(s => s.Get(id)).ReturnsAsync((Store)null!);

        // Act
        var result = await storeController.Delete(id);

        // Assert
        result.Should().BeOfType<NotFoundResult>();
    }

    [Test]
    public async Task DeleteConfirmed_Post_DeletesStoreAndRedirects()
    {
        // Arrange
        var id = Guid.NewGuid();

        // Act
        var result = await storeController.DeleteConfirmed(id);
        var redirectResult = result as RedirectToActionResult;

        // Assert
        storeServiceMock.Verify(s => s.Delete(id), Times.Once);
        redirectResult.Should().NotBeNull();
        redirectResult!.ActionName.Should().Be(nameof(StoreController.Index));
    }

    [Test]
    public void CreateProduct_Get_SetsStoreIdInViewBag()
    {
        // Arrange
        var storeId = Guid.NewGuid();

        // Act
        var result = storeController.CreateProduct(storeId);
        var viewResult = result as ViewResult;

        // Assert
        viewResult.Should().NotBeNull();
        ((Guid)storeController.ViewBag.StoreId).Should().Be(storeId);
    }

    [Test]
    public async Task CreateProduct_Post_ValidModel_RedirectsToDetails()
    {
        // Arrange
        var product = ProductGenerator.Generate().WithStore();

        // Act
        var result = await storeController.CreateProduct(product);
        var redirectResult = result as RedirectToActionResult;

        // Assert
        productServiceMock.Verify(s => s.Create(It.Is<Product>(p => p.Name == product.Name)), Times.Once);
        redirectResult.Should().NotBeNull();
        redirectResult!.ActionName.Should().Be("Details");
        redirectResult.RouteValues!["storeId"].Should().Be(product.StoreId);
    }

    [Test]
    public async Task CreateProduct_Post_InvalidModel_ReturnsSameView()
    {
        // Arrange
        var product = ProductGenerator.Generate();
        storeController.ModelState.AddModelError("Name", "Required");

        // Act
        var result = await storeController.CreateProduct(product);
        var viewResult = result as ViewResult;

        // Assert
        viewResult.Should().NotBeNull();
        viewResult!.Model.Should().Be(product);
    }
}
