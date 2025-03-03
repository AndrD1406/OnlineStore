using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineStore.DataAccess.Repository.Base
{
    public class EntityRepository <T> where T : class
    {
        protected readonly OnlineStoreDbContext dbcontext;


    }
}
