using Microsoft.EntityFrameworkCore;
using OnlineStore.BusinessLogic.Services.Interfaces;
using OnlineStore.DataAccess;
using OnlineStore.DataAccess.Models;
using OnlineStore.DataAccess.Repository.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace OnlineStore.BusinessLogic.Services;

public class ProductService : IProductService
{
    private readonly IEntityRepository<Guid, Product> repository;

    public ProductService(IEntityRepository<Guid, Product> repository)
    {
        this.repository = repository;
    }

    public async Task<IEnumerable<Product>> GetAll()
    {
        return await this.repository.GetAll();
    }

    public async Task<Product> Create(Product product)
    {
        return await this.repository.Create(product);
    }

    public async Task<IEnumerable<Product>> GetByStore(Guid storeId)
    {
        return await repository.GetByFilter(x => x.StoreId == storeId);
    }

    public async Task<IEnumerable<Product>> Filter(Expression<Func<Product, bool>> expression, int page=-1, int pageSize=-1)
    {
        return await repository.GetByFilter(expression, page, pageSize, includeProperties: nameof(Product.Store));
    }

    public async Task<Product> UpdateProduct(Guid productId, Product product)
    {
        var productToUpdate = await this.repository.GetById(productId);
        if (productToUpdate == null)
        {
            throw new Exception("Product not found.");
        }

        productToUpdate.Name = product.Name;
        productToUpdate.Description = product.Description;
        productToUpdate.Price = product.Price;
        productToUpdate.Quantity = product.Quantity;

        var updatedProduct = await this.repository.Update(productToUpdate);
        return updatedProduct;
    }

    public async Task<int> Count (Expression<Func<Product, bool>>? expression)
    {
        return await repository.Count(expression);
    }
}
