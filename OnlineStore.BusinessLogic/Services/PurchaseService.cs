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
    private readonly IEntityRepository<Guid, PurchaseItem> purchaseItemRepository;

    public PurchaseService(
        IEntityRepository<Guid, Purchase> repository,
        IEntityRepository<Guid, PurchaseItem> purchaseItemRepository)
    {
        this.repository = repository;
        this.purchaseItemRepository = purchaseItemRepository;
    }

    public async Task<IEnumerable<Purchase>> GetAll()
        => await repository.GetAll();

    public async Task<Purchase> Create(Purchase purchase)
        => await repository.Create(purchase);

    public async Task<Purchase> CreateWithItems(
    Purchase purchase,
    IEnumerable<(Guid productId, int quantity, double unitPrice)> lines)
    {
        var created = await repository.Create(purchase);

        foreach (var (prodId, qty, price) in lines)
        {
            var pi = new PurchaseItem
            {
                Id = Guid.NewGuid(),
                PurchaseId = created.Id,
                ProductId = prodId,
                Quantity = qty,
                UnitPrice = price
            };
            await purchaseItemRepository.Create(pi);
        }

        return created;
    }

    public async Task<IEnumerable<CustomerSpend>> GetTopCustomersForProduct(Guid productId, int topN = 10)
    {
        var items = await purchaseItemRepository.GetByFilter(
            pi => pi.ProductId == productId,
            includeProperties: "Purchase,Purchase.User,Product");

        var top = items
            .Where(pi => pi.Purchase?.User != null)   // guard
            .GroupBy(pi => pi.Purchase!.User!)
            .Select(g => new CustomerSpend
            {
                User = g.Key!,
                TotalSpent = g.Sum(pi => pi.Quantity * pi.UnitPrice)
            })
            .OrderByDescending(x => x.TotalSpent)
            .Take(topN)
            .ToList();

        return top;
    }
}

