using OnlineStore.DataAccess;
using OnlineStore.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace OnlineStore.BusinessLogic.Services.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetByStore(Guid storeId);

        Task<Product> Create(Product product);

        Task<IEnumerable<Product>> GetAll();

        public Task<int> Count(Expression<Func<Product, bool>>? expression);

        Task<IEnumerable<Product>> Filter(Expression<Func<Product, bool>> expression, int page = -1, int pageSize = -1);

        Task<Product> UpdateProduct(Guid productId, Product product);
    }
}
