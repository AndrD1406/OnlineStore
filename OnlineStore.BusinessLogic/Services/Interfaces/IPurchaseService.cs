using OnlineStore.BusinessLogic.Dtos;
using OnlineStore.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace OnlineStore.BusinessLogic.Services.Interfaces;

public interface IPurchaseService
{
    Task<IEnumerable<Purchase>> GetAll();
    Task<Purchase> Create(Purchase purchase);
    Task<IEnumerable<CustomerSpend>> GetTopCustomersForProduct(Guid productId, int topN = 10);
    Task<Purchase> CreateWithItems(
    Purchase purchase,
    IEnumerable<(Guid productId, int quantity, double unitPrice)> lines);
    public Task<IEnumerable<Purchase>> Filter(Expression<Func<Purchase, bool>> expression, int page = -1, int pageSize = -1);
    public Task<int> Count(Expression<Func<Purchase, bool>>? expression);
}
