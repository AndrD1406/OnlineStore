using Microsoft.EntityFrameworkCore;
using OnlineStore.DataAccess;
using OnlineStore.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineStore.BusinessLogic.Services
{
    public class ProductService : IProductService
    {
        private readonly OnlineStoreDbContext context;
        public ProductService(OnlineStoreDbContext dbContext)
        {
            context = dbContext;
        }
        public async Task<IEnumerable<Product>> GetProductsByStore(Guid storeId)
        {
            return await context.Products.Where(x => x.StoreId == storeId).ToListAsync();
        }
    }
}
