namespace Ch04_LanguageFeatures.Models
{
    public class Product
    {
        public string Name { get; set; }
        public decimal? Price { get; set; }

        public static Product[] GetProducts()
        {
            Product kayak = new Product { Name = "Kayak", Price = 275m };
            Product lifejacket = new Product { Name = "Life Jacket", Price = 48.95m };

            return new[] { kayak, lifejacket, null };
        }
    }
}
