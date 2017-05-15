using System.Collections;
using System.Collections.Generic;

namespace Ch04_LanguageFeatures.Models
{
    public class ShoppingCart : IEnumerable<Product>
    {
        public List<Product> Products = new List<Product>();
        
        public IEnumerator<Product> GetEnumerator()
        {
            return Products.GetEnumerator();    
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
