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

namespace OnlineStore.BusinessLogic.Services
{
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

        public async Task<IEnumerable<Product>> GetByStore(Guid storeId)
        {
            return await repository.GetByFilter(x => x.StoreId == storeId);
        }

        public async Task<IEnumerable<Product>> Filter(Expression<Func<Product, bool>> expression)
        {
            return await repository.GetByFilter(expression, nameof(Product.Store));
        }

        //public async Task<Product> UpdateProduct(Guid productId, Product product)
        //{
        //    var productToUpdate = await context.Products.FirstOrDefaultAsync(x=>x.Id == productId);

        //    var updatedProduct = await this.repository.Update(productToUpdate);

        //    return updatedProduct;
        //}
    }
}
