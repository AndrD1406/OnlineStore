using OnlineStore.BusinessLogic.Dtos;
using OnlineStore.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineStore.BusinessLogic.Services.Interfaces;

public interface IPurchaseService
{
    Task<IEnumerable<Purchase>> GetAll();
    Task<Purchase> Create(Purchase purchase);
    Task<IEnumerable<CustomerSpend>> GetTopCustomersForProduct(Guid productId, int topN = 10);
}
