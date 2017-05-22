using System.Collections.Generic;

namespace SportsStore.Models
{
    public interface IProductRepository
    { 
        IEnumerable<Product> Products { get; }

        void DeleteProduct(int productID);
        void SaveProduct(Product product);
    }
}
