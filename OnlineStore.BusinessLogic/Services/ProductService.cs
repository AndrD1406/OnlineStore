using Microsoft.EntityFrameworkCore;
using OnlineStore.BusinessLogic.Services.Interfaces;
using OnlineStore.DataAccess;
using OnlineStore.DataAccess.Models;
using OnlineStore.DataAccess.Repository.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineStore.BusinessLogic.Services
{
    public class ProductService : IProductService
    {
        //private readonly IEntityRepository<Guid, Product> repository;
        private readonly OnlineStoreDbContext context;
        public ProductService(OnlineStoreDbContext dbContext)
        {
            context = dbContext;
        }

        public async Task<IEnumerable<Product>> GetProducts()
        {
            return await context.Products.ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProductsByStore(Guid storeId)
        {
            return await context.Products.Where(x => x.StoreId == storeId).ToListAsync();
        }

        //public async Task<Product> UpdateProduct(Guid productId, Product product)
        //{
        //    var productToUpdate = await context.Products.FirstOrDefaultAsync(x=>x.Id == productId);

        //    var updatedProduct = await this.repository.Update(productToUpdate);

        //    return updatedProduct;
        //}
    }
}
