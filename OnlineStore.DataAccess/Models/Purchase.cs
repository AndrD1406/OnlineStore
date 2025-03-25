using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineStore.DataAccess.Models;

public class Purchase : IKeyedEntity<Guid>
{
    [Key]
    public Guid Id { get; set; }

    public IEnumerable<Product>? Products { get; set; }

    [ForeignKey(nameof(ApplicationUser))]
    public Guid UserId { get; set; }

    public ApplicationUser? User { get; set; }

    public DateTime CreatedDate { get; set; }

    public double TotalAmount { get; set; }

    public Purchase() { }
}
