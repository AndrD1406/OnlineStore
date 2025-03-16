using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineStore.DataAccess.Models
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string? Name { get; set; }
        public IEnumerable<Purchase>? Purchases { get; set; }
        public bool IsAdmin { get; set; }

    }
}
