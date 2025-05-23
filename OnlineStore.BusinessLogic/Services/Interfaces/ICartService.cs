﻿using OnlineStore.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace OnlineStore.BusinessLogic.Services.Interfaces;

public interface ICartService
{
    Task<IEnumerable<Cart>> GetByUserId(Guid userId);
    Task<ProductToCart> CreateCartProduct(ProductToCart cartProduct);
    Task<Cart> Create(Cart cart);
    Task Delete(Guid id);
    Task<Cart> AddProductToCart(Guid id, ProductToCart cartProduct);
    Task RemoveProductFromCart(Guid cartId, Guid productId);
    Task ClearCart(Guid cartId);
    Task<int> Count(Expression<Func<Cart, bool>>? expression = null);
    Task<IEnumerable<Cart>> Filter(Expression<Func<Cart, bool>> expression, int page = -1, int pageSize = -1);

}
