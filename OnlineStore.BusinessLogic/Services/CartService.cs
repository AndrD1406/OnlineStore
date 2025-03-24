using OnlineStore.BusinessLogic.Services.Interfaces;
using OnlineStore.DataAccess.Models;
using OnlineStore.DataAccess.Repository.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineStore.BusinessLogic.Services;

public class CartService : ICartService
{
    private readonly IEntityRepository<Guid, Cart> repository;

    public CartService(IEntityRepository<Guid, Cart> repository)
    {
        this.repository = repository;
    }


}
