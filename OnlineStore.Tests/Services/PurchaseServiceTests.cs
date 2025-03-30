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
public class PurchaseServiceTests
{
    private Mock<IEntityRepository<Guid, Purchase>> _repositoryMock;
    private PurchaseService _purchaseService;

    [SetUp]
    public void Setup()
    {
        _repositoryMock = new Mock<IEntityRepository<Guid, Purchase>>();
        _purchaseService = new PurchaseService(_repositoryMock.Object);
    }

    [Test]
    public async Task GetAll_WhenCalled_ReturnsAllPurchases()
    {
        // Arrange
        var purchases = PurchaseGenerator.Generate(5);
        _repositoryMock.Setup(r => r.GetAll()).ReturnsAsync(purchases);

        // Act
        var result = await _purchaseService.GetAll();

        // Assert
        result.Should().BeEquivalentTo(purchases);
        _repositoryMock.Verify(r => r.GetAll(), Times.Once);
    }

    [Test]
    public async Task Create_WhenCalled_CreatesAndReturnsPurchase()
    {
        // Arrange
        var purchase = PurchaseGenerator.Generate();
        _repositoryMock.Setup(r => r.Create(It.IsAny<Purchase>())).ReturnsAsync(purchase);

        // Act
        var result = await _purchaseService.Create(purchase);

        // Assert
        result.Should().BeEquivalentTo(purchase);
        _repositoryMock.Verify(r => r.Create(It.IsAny<Purchase>()), Times.Once);
    }
}

