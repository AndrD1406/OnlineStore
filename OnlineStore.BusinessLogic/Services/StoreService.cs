using OnlineStore.BusinessLogic.Services.Interfaces;
using OnlineStore.DataAccess.Models;
using OnlineStore.DataAccess.Repository.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace OnlineStore.BusinessLogic.Services;

public class StoreService : IStoreService
{
    private readonly IEntityRepository<Guid, Store> repository;

    public StoreService(IEntityRepository<Guid, Store> repository)
    {
        this.repository = repository;
    }

    public async Task<Store> Get(Guid id)
    {
        return await this.repository.GetById(id);
    }

    public async Task<IEnumerable<Store>> GetAll()
    {
        return await this.repository.GetAll();
    }

    public async Task<Store> Create(Store store)
    {
        store.Id = Guid.NewGuid();
        return await this.repository.Create(store);
    }

    public async Task Delete(Guid id)
    {
        var store = await this.repository.GetById(id);
        await this.repository.Delete(store);
    }

    public async Task<Store> Update(Guid storeId, Store store)
    {
        var storeToUpdate = await this.repository.GetById(storeId);

        storeToUpdate.Name = store.Name;

        var updatedStore = await this.repository.Update(storeToUpdate);

        return updatedStore;
    }

    public async Task<int> Count(Expression<Func<Store, bool>>? expression = null)
    {
        return await repository.Count(expression);
    }

    public async Task<IEnumerable<Store>> Filter(Expression<Func<Store, bool>> expression, int page = -1, int pageSize = -1)
    {
        return await repository.GetByFilter(expression, page, pageSize, includeProperties: nameof(Store.Products));
    }
}
