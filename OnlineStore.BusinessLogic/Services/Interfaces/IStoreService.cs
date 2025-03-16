using OnlineStore.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineStore.BusinessLogic.Services.Interfaces
{
    public interface IStoreService
    {
        Task<Store> Get(Guid id);

        Task<IEnumerable<Store>> GetAll();

        Task Delete(Guid id);

        Task<Store> Update(Guid storeId, Store store);
    }
}
