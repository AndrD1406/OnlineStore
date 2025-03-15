using OnlineStore.BusinessLogic.Services.Interfaces;
using OnlineStore.DataAccess.Models;
using OnlineStore.DataAccess.Repository.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineStore.BusinessLogic.Services
{
    public class StoreService: IStoreService
    {
        private readonly IEntityRepository<Guid, Store> repository;

        public StoreService(IEntityRepository<Guid, Store> repository)
        {
            this.repository = repository;
        }
        public async Task<IEnumerable<Store>> GetAll()
        {
            return await this.repository.GetAll();
        }
    }
}
