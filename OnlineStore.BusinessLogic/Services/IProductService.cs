using OnlineStore.DataAccess;
using OnlineStore.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineStore.BusinessLogic.Services
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetProductsByStore(Guid storeId);
        Task<IEnumerable<Product>> GetProducts();
    }
}
