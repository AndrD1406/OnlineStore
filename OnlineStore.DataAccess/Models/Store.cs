using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineStore.DataAccess.Models
{
    public class Store : IKeyedEntity<Guid>
    {
        [Key]
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public IEnumerable<Product>? Products { get; set; }

        public Store()
        {
            
        }
    }
}
