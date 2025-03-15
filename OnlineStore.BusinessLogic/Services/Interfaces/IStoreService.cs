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
        Task<IEnumerable<Store>> GetAll();
    }
}
