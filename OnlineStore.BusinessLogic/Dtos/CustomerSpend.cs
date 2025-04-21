using OnlineStore.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineStore.BusinessLogic.Dtos;

public class CustomerSpend
{
    public ApplicationUser User { get; set; }

    public double TotalSpent { get; set; }

}
