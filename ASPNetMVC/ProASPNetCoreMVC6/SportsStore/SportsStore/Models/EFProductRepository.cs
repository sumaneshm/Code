using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsStore.Models
{
    public class EFProductRepository : IProductRepository
    {
        private ApplicationDbContext context;

        public EFProductRepository(ApplicationDbContext context) => this.context = context;

        public IEnumerable<Product> Products => context.Products;

        public void DeleteProduct(int productID)
        {
            var productToDelete = Products.FirstOrDefault(p => p.ProductID == productID);
            if (productToDelete != null)
            {
                context.Products.Remove(productToDelete);
                context.SaveChanges();
            }
        }

        public void SaveProduct(Product product)
        {
            if(product.ProductID != 0)
            {
                var prodToEdit = context.Products.FirstOrDefault(p => p.ProductID == product.ProductID);
                if(prodToEdit != null)
                {
                    prodToEdit.Name = product.Name;
                    prodToEdit.Category = product.Category;
                    prodToEdit.Description = product.Description;
                    prodToEdit.Price = product.Price;
                }
            }
            else {
                context.Products.Add(product);
            }

            context.SaveChanges();
        }
    }
}
