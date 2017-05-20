using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace SportsStore.Models
{
    public class Cart
    {
        public class CartLine
        {
            public int CartLineID { get; set; }
            public Product Product { get; set; }
            public int Quantity { get; set; }
        }

        private readonly List<CartLine> cartLines = new List<CartLine>();

        public IEnumerable<CartLine> Lines => cartLines;

        public virtual void AddLine(Product product, int quantity)
        {
            var line = cartLines.FirstOrDefault(l => l.Product.ProductID == product.ProductID);

            if (line != null)
            {
                line.Quantity += quantity;
            }
            else
            {
                cartLines.Add(new CartLine { Product = product, Quantity = quantity });
            }
        }

        public virtual void RemoveLine(Product product) =>
            cartLines.RemoveAll(l => l.Product.ProductID == product.ProductID);

        public virtual void Clear() => cartLines.Clear();

        public decimal Total => cartLines.Sum(c => c.Product.Price * c.Quantity);
    }
}
