using System.Collections.Generic;

namespace Ch06_WorkingWithVisualStudio.Models
{
    public class Repository : IRepository
    {
        private Dictionary<string, Product> products;
        private static Repository sharedRepository = new Repository();

        public static Repository SharedRepository => sharedRepository;
        public IEnumerable<Product> Products => products.Values;

        public Repository()
        {
            products = new Dictionary<string, Product>
            {
                ["Kayak"] = new Product { Name = "Kayak", Price = 275m },
                ["Life Jacket"] = new Product { Name = "Life Jacket", Price = 48.95m },
                ["Soccer ball"] = new Product { Name = "Soccer ball", Price = 19.5m },
                ["Corner flag"] = new Product { Name = "Corner flag", Price = 34.95m },
                ["Cricket bat"] = new Product { Name = "Cricket Bat", Price = 85.98m }
            };
        }

        public void AddProduct(Product p) => products.Add(p.Name, p);
    }
}
