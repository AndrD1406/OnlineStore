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
    private readonly IEntityRepository<Guid, CartProduct> cartProductRepository;

    public CartService(IEntityRepository<Guid, Cart> repository, IEntityRepository<Guid, CartProduct> cartProductRepository)
    {
        this.repository = repository;
        this.cartProductRepository = cartProductRepository;
    }

    public async Task<IEnumerable<Cart>> GetByUserId(Guid userId)
    {
        return await this.repository.GetByFilter(c => c.UserId == userId);
    }

    public async Task<CartProduct> CreateCartProduct(CartProduct cartProduct)
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

    public async Task<Cart> AddProductToCart(Guid id, CartProduct cartProduct)
    {
        var cart = await this.repository.GetById(id);

        if (cart.CartProducts != null)
        {
            cart.CartProducts.Add(cartProduct); 
        }
        
        return await this.repository.Update(cart);
    }


}
