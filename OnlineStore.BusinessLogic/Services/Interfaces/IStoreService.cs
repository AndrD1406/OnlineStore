using OnlineStore.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace OnlineStore.BusinessLogic.Services.Interfaces
{
    public interface IStoreService
    {
        Task<Store> Get(Guid id);

        Task<IEnumerable<Store>> GetAll();

        Task<Store> Create(Store store);

        Task Delete(Guid id);

        Task<Store> Update(Guid storeId, Store store);

        Task<int> Count(Expression<Func<Store, bool>>? expression = null);

        Task<IEnumerable<Store>> Filter(Expression<Func<Store, bool>> expression, int page = -1, int pageSize = -1);
    }
}
