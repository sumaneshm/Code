namespace Ch04_LanguageFeatures.Models
{
    public class Product
    {
        public string Name { get; set; }
        public decimal? Price { get; set; }
        public Product Related { get; set; }

        public static Product[] GetProducts()
        {
            Product kayak = new Product { Name = "Kayak", Price = 275m };
            Product lifejacket = new Product { Name = "Life Jacket", Price = 40.28m };
            kayak.Related = lifejacket;
            Product soccer = new Product { Name = "Soccer", Price = 25m };
            Product cornerFlag = new Product { Name = "Corner flag", Price = 23m };

            return new[] { kayak, lifejacket, soccer, cornerFlag };
        }
    }
}
