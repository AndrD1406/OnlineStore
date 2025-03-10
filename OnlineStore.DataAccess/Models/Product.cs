using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineStore.DataAccess.Models
{
    public class Product: IKeyedEntity<Guid>
    {
        [Key]
        public Guid Id { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }

        public int Quantity { get; set; }

        public double Price { get; set; }

        [ForeignKey(nameof(Store))]

        public Guid StoreId { get; set; }

        public Store? Store { get; set; }

        [ForeignKey(nameof(Purchase))]
        public Guid PurchaseId { get; set; }

        public Purchase? Purchase { get; set; }

        public Product()
        {
        }
    }
}
