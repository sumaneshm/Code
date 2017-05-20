using System.Collections.Generic;

namespace SportsStore.Models
{
    public class FakeProductRepository : IProductRepository
    {
        public IEnumerable<Product> Products => new List<Product>
        {
            new Product {Name = "Football", Price = 24m},
            new Product {Name="Cricket bat", Price=40m},
            new Product {Name="Baseball bat", Price=33m}
        };
    }
}
