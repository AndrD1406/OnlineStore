using OnlineStore.DataAccess.Models;

namespace OnlineStore.Models
{
    public class StoreDetailsViewModel
    {
        public Store Store { get; set; }
        public IEnumerable<Product> Products { get; set; }
        public string? ProductFilter { get; set; }
        public double? PriceFilter { get; set; }
    }
}
