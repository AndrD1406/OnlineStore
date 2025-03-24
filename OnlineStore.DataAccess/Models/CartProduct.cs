using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineStore.DataAccess.Models;

public class CartProduct : IKeyedEntity<Guid>
{
    [Key]
    public Guid Id { get; set; }

    [ForeignKey(nameof(Product))]
    public Guid ProductId { get; set; }

    public Product? Product { get; set; }
    
    public int Quantity { get; set; }

    public CartProduct() { }
}
