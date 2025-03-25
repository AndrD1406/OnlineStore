using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineStore.DataAccess.Models;

public class Cart : IKeyedEntity<Guid>
{
    [Key]
    public Guid Id { get; set; }

    [ForeignKey(nameof(ApplicationUser))]
    public Guid UserId { get; set; }

    public ApplicationUser? User { get; set; }

    public List<ProductToCart>? ProductToCarts { get; set; } = new List<ProductToCart>();

    public Cart() { }
}
