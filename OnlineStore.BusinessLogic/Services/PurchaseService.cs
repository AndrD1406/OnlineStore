using OnlineStore.BusinessLogic.Dtos;
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
        => await repository.GetAll();

    public async Task<Purchase> Create(Purchase purchase)
        => await repository.Create(purchase);

    public async Task<IEnumerable<CustomerSpend>> GetTopCustomersForProduct(Guid productId, int topN = 10)
    {
        var all = await repository.GetAllWithDetails("Products,User");
        return all
            .SelectMany(p => p.Products
                .Where(prod => prod.Id == productId)
                .Select(prod => new { p.User, prod.Price }))
            .GroupBy(x => x.User)
            .Select(g => new CustomerSpend
            {
                User = g.Key,
                TotalSpent = g.Sum(x => x.Price)
            })
            .OrderByDescending(x => x.TotalSpent)
            .Take(topN)
            .ToList();
    }
}
