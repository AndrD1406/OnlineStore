using OnlineStore.BusinessLogic.Services.Interfaces;
using OnlineStore.DataAccess.Models;
using OnlineStore.DataAccess.Repository.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineStore.BusinessLogic.Services;

public class PurchaseService : IPurchaseService
{
    private readonly IEntityRepository<Guid, Purchase> repository;

    public PurchaseService(IEntityRepository<Guid, Purchase> repository)
    {
        this.repository = repository;
    }

    public async Task<IEnumerable<Purchase>> GetAll()
    {
        return await this.repository.GetAll();
    }

    public async Task<Purchase> Create(Purchase purchase)
    {
        return await this.repository.Create(purchase);
    }
}
