using OnlineStore.BusinessLogic.Services.Interfaces;
using OnlineStore.DataAccess.Models;
using OnlineStore.DataAccess.Repository.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineStore.BusinessLogic.Services;

public class CartService : ICartService
{
    private readonly IEntityRepository<Guid, Cart> repository;
    private readonly IEntityRepository<Guid, ProductToCart> cartProductRepository;

    public CartService(IEntityRepository<Guid, Cart> repository, IEntityRepository<Guid, ProductToCart> cartProductRepository)
    {
        this.repository = repository;
        this.cartProductRepository = cartProductRepository;
    }

    public async Task<IEnumerable<Cart>> GetByUserId(Guid userId)
    {
        return await repository.GetByFilter(
            c => c.UserId == userId,
            includeProperties: "ProductToCarts.Product"
        );
    }

    public async Task<ProductToCart> CreateCartProduct(ProductToCart cartProduct)
    {
        return await this.cartProductRepository.Create(cartProduct);
    }

    public async Task<Cart> Create(Cart cart)
    {
        return await this.repository.Create(cart);
    }

    public async Task Delete(Guid id)
    {
        var cart = await this.repository.GetById(id);
        await this.repository.Delete(cart);
    }

    public async Task<Cart> AddProductToCart(Guid id, ProductToCart cartProduct)
    {
        var cart = await this.repository.GetById(id);
        if (cart.ProductToCarts == null)
            cart.ProductToCarts = new List<ProductToCart>();

        await this.cartProductRepository.Create(cartProduct);
        return cart;
    }

    public async Task RemoveProductFromCart(Guid cartId, Guid productId)
    {
        var associations = await this.cartProductRepository.GetByFilter(
            ptc => ptc.CartId == cartId && ptc.ProductId == productId
        );
        foreach (var association in associations)
        {
            await this.cartProductRepository.Delete(association);
        }
    }

    public async Task ClearCart(Guid cartId)
    {
        var items = await this.cartProductRepository.GetByFilter(ptc => ptc.CartId == cartId);
        foreach (var item in items)
        {
            await this.cartProductRepository.Delete(item);
        }
    }

    public async Task<Cart> UpdateCoupon(Guid cartId, string? coupon)
    {
        var cart = await repository.GetById(cartId);

        cart.Coupon = coupon;
        await repository.Update(cart);
        return cart;
    }
}
