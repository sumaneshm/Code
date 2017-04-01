using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace C19_BasicValidation
{
    public class StoreDB
    {
        public ObservableCollection<Product> Products = new ObservableCollection<Product>();

        public StoreDB()
        {
            Products.Add(new Product() { ProductId = 1, ModelName = "Maruthi 800", ModelNumber = "M800", UnitPrice = 20000, Description = "Long lasting low cost car which was ruling India for more than 3 decades" });
            Products.Add(new Product() { ProductId = 2, ModelName = "Maruthi 1000", ModelNumber = "M1000", UnitPrice = 80000, Description = "Long lasting medium cost car from reliable Maruthi" });
            Products.Add(new Product() { ProductId = 3, ModelName = "Icon", ModelNumber = "F10", UnitPrice = 80000, Description = "A quality car from Ford" });
            Products.Add(new Product() { ProductId = 4, ModelName = "Mercedes Benz", ModelNumber = "BeNZ3", UnitPrice = 700000, Description = "Cozy, comfortable car from the most highly reputed car" });
            Products.Add(new Product() { ProductId = 5, ModelName = "Toyota", ModelNumber = "ToyStory5", UnitPrice = 50000, Description = "Japan manufactures enters Indian market" });
        }

        public Product GetProduct(int ProductId)
        {
            // var result = from prod in products where prod.ModelNumber.Equals(ModelNumber) select prod;
            return Products.FirstOrDefault(t => t.ProductId == ProductId);
        }

        public ICollection<Product> GetAllProducts()
        {
            //ObservableCollection<Product> obsProducts = new ObservableCollection<Product>();
            //foreach (Product prd in Products)
            //{
            //    obsProducts.Add(prd);
            //}

            return Products;
        }

        public void UpdateProduct(Product product)
        {
            Product updateProduct = GetProduct(product.ProductId);
            if (updateProduct != null)
            {
                updateProduct.ModelName = product.ModelName;
                updateProduct.ModelNumber = product.ModelNumber;
                updateProduct.Description = product.Description;
                //updateProduct.UnitPrice *= 1.1M;
            }

        }

        public void DeleteProduct(Product DelProduct)
        {
            Products.Remove(DelProduct);
        }
    }
}
