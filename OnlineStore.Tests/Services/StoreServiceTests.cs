using Bogus;
using FluentAssertions;
using Moq;
using OnlineStore.BusinessLogic.Services;
using OnlineStore.DataAccess.Models;
using OnlineStore.DataAccess.Repository.Base;
using OnlineStore.Tests.TestDataGenerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineStore.Tests.Services;

[TestFixture]
public class StoreServiceTests
{
    private Mock<IEntityRepository<Guid, Store>> _repositoryMock;
    private StoreService _storeService;

    [SetUp]
    public void Setup()
    {
        _repositoryMock = new Mock<IEntityRepository<Guid, Store>>();
        _storeService = new StoreService(_repositoryMock.Object);
    }

    [Test]
    public async Task Get_WhenCalled_ReturnsStore()
    {
        // Arrange
        var storeId = Guid.NewGuid();
        var store = StoreGenerator.Generate().WithProducts(3);
        store.Id = storeId;
        _repositoryMock.Setup(r => r.GetById(storeId))
                       .ReturnsAsync(store);

        // Act
        var result = await _storeService.Get(storeId);

        // Assert
        result.Should().BeEquivalentTo(store);
        _repositoryMock.Verify(r => r.GetById(storeId), Times.Once);
    }

    [Test]
    public async Task GetAll_WhenCalled_ReturnsAllStores()
    {
        // Arrange
        var stores = StoreGenerator.Generate(5);
        _repositoryMock.Setup(r => r.GetAll())
                       .ReturnsAsync(stores);

        // Act
        var result = await _storeService.GetAll();

        // Assert
        result.Should().BeEquivalentTo(stores);
        _repositoryMock.Verify(r => r.GetAll(), Times.Once);
    }

    [Test]
    public async Task Create_WhenCalled_CreatesAndReturnsStore()
    {
        // Arrange
        var store = StoreGenerator.Generate();
        // The service sets a new ID, so we ensure the incoming store has an empty ID.
        store.Id = Guid.Empty;

        _repositoryMock.Setup(r => r.Create(It.IsAny<Store>()))
                       .ReturnsAsync((Store s) => s);

        // Act
        var result = await _storeService.Create(store);

        // Assert
        result.Id.Should().NotBeEmpty();
        result.Should().BeEquivalentTo(store, options => options.Excluding(s => s.Id));
        _repositoryMock.Verify(r => r.Create(It.IsAny<Store>()), Times.Once);
    }

    [Test]
    public async Task Delete_WhenCalled_DeletesStore()
    {
        // Arrange
        var storeId = Guid.NewGuid();
        var store = StoreGenerator.Generate();
        store.Id = storeId;
        _repositoryMock.Setup(r => r.GetById(storeId))
                       .ReturnsAsync(store);
        _repositoryMock.Setup(r => r.Delete(store))
                       .Returns(Task.CompletedTask);

        // Act
        await _storeService.Delete(storeId);

        // Assert
        _repositoryMock.Verify(r => r.GetById(storeId), Times.Once);
        _repositoryMock.Verify(r => r.Delete(store), Times.Once);
    }

    [Test]
    public async Task Update_WhenCalled_UpdatesStoreName()
    {
        // Arrange
        var storeId = Guid.NewGuid();
        var originalStore = StoreGenerator.Generate();
        originalStore.Id = storeId;
        originalStore.Name = "Original Name";

        var updateData = new Store { Name = "Updated Name" };

        var updatedStore = StoreGenerator.Generate();
        updatedStore.Id = storeId;
        updatedStore.Name = "Updated Name";

        _repositoryMock.Setup(r => r.GetById(storeId))
                       .ReturnsAsync(originalStore);
        _repositoryMock.Setup(r => r.Update(It.IsAny<Store>()))
                       .ReturnsAsync(updatedStore);

        // Act
        var result = await _storeService.Update(storeId, updateData);

        // Assert
        result.Name.Should().Be("Updated Name");
        _repositoryMock.Verify(r => r.GetById(storeId), Times.Once);
        _repositoryMock.Verify(r => r.Update(It.Is<Store>(s => s.Name == "Updated Name")), Times.Once);
    }
}

