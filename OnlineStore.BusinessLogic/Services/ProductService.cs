using Microsoft.EntityFrameworkCore;
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
        private readonly EntityRepository<Guid, Product> repository;
        public ProductService(EntityRepository<Guid, Product> repository)
        {
            this.repository = repository;
        }
        public async Task<IEnumerable<Product>> GetProductsByStore(Guid storeId)
        {
            return await repository.GetByFilter(x => x.StoreId == storeId);
        }
    }
}
