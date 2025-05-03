using Bogus;
using Moq;
using OnlineStore.BusinessLogic.Services;
using OnlineStore.DataAccess.Models;
using OnlineStore.DataAccess.Repository.Base;
using FluentAssertions;
using System.Linq.Expressions;
using OnlineStore.BusinessLogic.Services.Interfaces;

namespace OnlineStore.Tests.Services;

public class CartServiceTests
{
    private Mock<IEntityRepository<Guid, Cart>> _cartRepositoryMock;
    private Mock<IEntityRepository<Guid, ProductToCart>> _cartProductRepositoryMock;
    private ICartService _cartService;
    private Faker _faker;

    [SetUp]
    public void Setup()
    {
        _cartRepositoryMock = new Mock<IEntityRepository<Guid, Cart>>();
        _cartProductRepositoryMock = new Mock<IEntityRepository<Guid, ProductToCart>>();
        _cartService = new CartService(_cartRepositoryMock.Object, _cartProductRepositoryMock.Object);
        _faker = new Faker();
    }

    private void SetupGetByUserId(IEnumerable<Cart> carts)
    {
        _cartRepositoryMock
            .Setup(repo => repo.GetByFilter(
                It.IsAny<Expression<Func<Cart, bool>>>(), -1, -1,
                It.IsAny<string>()))
            .ReturnsAsync(carts);
    }

    private void SetupCreateCartProduct(ProductToCart cartProduct)
    {
        _cartProductRepositoryMock
            .Setup(repo => repo.Create(It.IsAny<ProductToCart>()))
            .ReturnsAsync(cartProduct);
    }

    private void SetupCreate(Cart cart)
    {
        _cartRepositoryMock
            .Setup(repo => repo.Create(It.IsAny<Cart>()))
            .ReturnsAsync(cart);
    }

    private void SetupDelete(Cart cart)
    {
        _cartRepositoryMock
            .Setup(repo => repo.GetById(cart.Id))
            .ReturnsAsync(cart);
        _cartRepositoryMock
            .Setup(repo => repo.Delete(cart))
            .Returns(Task.CompletedTask);
    }

    private void SetupAddProductToCart(Cart cart, ProductToCart cartProduct)
    {
        _cartRepositoryMock
            .Setup(repo => repo.GetById(cart.Id))
            .ReturnsAsync(cart);
        _cartProductRepositoryMock
            .Setup(repo => repo.Create(It.IsAny<ProductToCart>()))
            .ReturnsAsync(cartProduct);
    }

    private void SetupRemoveProductFromCart(IEnumerable<ProductToCart> associations)
    {
        _cartProductRepositoryMock
            .Setup(repo => repo.GetByFilter(
                It.IsAny<Expression<Func<ProductToCart, bool>>>(), -1, -1,
                It.IsAny<string>()))
            .ReturnsAsync(associations);
        _cartProductRepositoryMock
            .Setup(repo => repo.Delete(It.IsAny<ProductToCart>()))
            .Returns(Task.CompletedTask);
    }

    private void SetupClearCart(IEnumerable<ProductToCart> items)
    {
        _cartProductRepositoryMock
            .Setup(repo => repo.GetByFilter(
                It.IsAny<Expression<Func<ProductToCart, bool>>>(), -1, -1,
                It.IsAny<string>()))
            .ReturnsAsync(items);
        _cartProductRepositoryMock
            .Setup(repo => repo.Delete(It.IsAny<ProductToCart>()))
            .Returns(Task.CompletedTask);
    }

    [Test]
    public async Task GetByUserId_WhenCalled_ReturnsCartsForUser()
    {
        var userId = Guid.NewGuid();
        var carts = new List<Cart>
        {
            new Cart { Id = Guid.NewGuid(), UserId = userId, ProductToCarts = new List<ProductToCart>() },
            new Cart { Id = Guid.NewGuid(), UserId = userId, ProductToCarts = new List<ProductToCart>() }
        };

        SetupGetByUserId(carts);

        var result = await _cartService.GetByUserId(userId);

        result.Should().BeEquivalentTo(carts);
        _cartRepositoryMock.Verify(
            repo => repo.GetByFilter(It.IsAny<Expression<Func<Cart, bool>>>(), -1, -1, It.IsAny<string>()),
            Times.Once);
    }

    [Test]
    public async Task CreateCartProduct_WhenCalled_ReturnsCreatedProductToCart()
    {
        var productToCart = new ProductToCart
        {
            Id = Guid.NewGuid(),
            ProductId = Guid.NewGuid(),
            CartId = Guid.NewGuid(),
            Quantity = _faker.Random.Int(1, 10)
        };

        SetupCreateCartProduct(productToCart);

        var result = await _cartService.CreateCartProduct(productToCart);

        result.Should().BeEquivalentTo(productToCart);
        _cartProductRepositoryMock.Verify(
            repo => repo.Create(It.IsAny<ProductToCart>()),
            Times.Once);
    }

    [Test]
    public async Task Create_WhenCalled_ReturnsCreatedCart()
    {
        var cart = new Cart { Id = Guid.NewGuid(), UserId = Guid.NewGuid(), ProductToCarts = new List<ProductToCart>() };
        SetupCreate(cart);

        var result = await _cartService.Create(cart);

        result.Should().BeEquivalentTo(cart);
        _cartRepositoryMock.Verify(repo => repo.Create(It.IsAny<Cart>()), Times.Once);
    }

    [Test]
    public async Task Delete_WhenCalled_CallsRepositoryDelete()
    {
        var cart = new Cart { Id = Guid.NewGuid(), UserId = Guid.NewGuid() };
        SetupDelete(cart);

        await _cartService.Delete(cart.Id);

        _cartRepositoryMock.Verify(repo => repo.GetById(cart.Id), Times.Once);
        _cartRepositoryMock.Verify(repo => repo.Delete(cart), Times.Once);
    }

    [Test]
    public async Task AddProductToCart_WhenCalled_ReturnsCartWithProductAdded()
    {
        var cartId = Guid.NewGuid();
        var cart = new Cart { Id = cartId, UserId = Guid.NewGuid(), ProductToCarts = new List<ProductToCart>() };
        var productToCart = new ProductToCart
        {
            Id = Guid.NewGuid(),
            ProductId = Guid.NewGuid(),
            CartId = cartId,
            Quantity = _faker.Random.Int(1, 10)
        };

        SetupAddProductToCart(cart, productToCart);

        var result = await _cartService.AddProductToCart(cartId, productToCart);

        result.Should().BeEquivalentTo(cart);
        _cartRepositoryMock.Verify(repo => repo.GetById(cartId), Times.Once);
        _cartProductRepositoryMock.Verify(
            repo => repo.Create(It.Is<ProductToCart>(ptc => ptc.Id == productToCart.Id)),
            Times.Once);
    }

    [Test]
    public async Task RemoveProductFromCart_WhenCalled_DeletesAllMatchingAssociations()
    {
        var cartId = Guid.NewGuid();
        var productId = Guid.NewGuid();
        var associations = new List<ProductToCart>
        {
            new ProductToCart { Id = Guid.NewGuid(), CartId = cartId, ProductId = productId, Quantity = 2 },
            new ProductToCart { Id = Guid.NewGuid(), CartId = cartId, ProductId = productId, Quantity = 3 }
        };

        SetupRemoveProductFromCart(associations);

        await _cartService.RemoveProductFromCart(cartId, productId);

        _cartProductRepositoryMock.Verify(
            repo => repo.GetByFilter(It.IsAny<Expression<Func<ProductToCart, bool>>>(), -1, -1, It.IsAny<string>()),
            Times.Once);
        _cartProductRepositoryMock.Verify(
            repo => repo.Delete(It.IsAny<ProductToCart>()),
            Times.Exactly(associations.Count));
    }

    [Test]
    public async Task ClearCart_WhenCalled_DeletesAllItemsInCart()
    {
        var cartId = Guid.NewGuid();
        var items = new List<ProductToCart>
        {
            new ProductToCart { Id = Guid.NewGuid(), CartId = cartId, ProductId = Guid.NewGuid(), Quantity = 1 },
            new ProductToCart { Id = Guid.NewGuid(), CartId = cartId, ProductId = Guid.NewGuid(), Quantity = 2 }
        };

        SetupClearCart(items);

        await _cartService.ClearCart(cartId);

        _cartProductRepositoryMock.Verify(
            repo => repo.GetByFilter(It.IsAny<Expression<Func<ProductToCart, bool>>>(), -1, -1, It.IsAny<string>()),
            Times.Once);
        _cartProductRepositoryMock.Verify(
            repo => repo.Delete(It.IsAny<ProductToCart>()),
            Times.Exactly(items.Count));
    }
}


