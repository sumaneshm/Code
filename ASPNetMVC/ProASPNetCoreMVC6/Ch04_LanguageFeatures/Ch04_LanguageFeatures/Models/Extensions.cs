using System;
using System.Collections.Generic;
using System.Linq;

namespace Ch04_LanguageFeatures.Models
{
    public static class Extensions
    {
        public static decimal Total(this IEnumerable<Product> products)
        {
            return products.Sum(p => p?.Price ?? 0);
        }

        public static IEnumerable<Product> Filter(this IEnumerable<Product> products, 
            Func<Product, bool>filterFunc)
        {
            foreach(Product p in products)
            {
                if (filterFunc(p))
                    yield return p;
            }
        }
    }
}
