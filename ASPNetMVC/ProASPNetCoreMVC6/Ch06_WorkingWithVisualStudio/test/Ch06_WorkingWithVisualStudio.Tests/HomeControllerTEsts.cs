using Ch06_WorkingWithVisualStudio.Controllers;
using Ch06_WorkingWithVisualStudio.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Xunit;
using System;
using System.Collections;

namespace Ch06_WorkingWithVisualStudio.Tests
{
    public class HomeControllerTests
    {
        public class ProductTestData : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[] { GetProductsAbovePrice50() };
                yield return new object[] { GetProductsUnderPrice50() };
            }

            private IEnumerable<Product> GetProductsAbovePrice50()
            {
                var prices = new[] { 55.0m, 323.3m, 434.23m, 51.2m };
                foreach (var p in prices)
                {
                    yield return new Product { Name = "Price" + p, Price = p };
                }
            }

            private IEnumerable<Product> GetProductsUnderPrice50()
            {
                return new[]{
                new Product {Name="P1", Price=25m},
                new Product {Name="P2", Price=23m},
                new Product {Name="P3", Price=15m},
                new Product {Name="P4", Price=12m},
                };
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }


        class MockRepository : IRepository
        {
            public IEnumerable<Product> Products { get; set; }
            public void AddProduct(Product p)
            {
                throw new NotImplementedException();
            }
        }

        [Theory]
        [ClassData(typeof(ProductTestData))]
        public void IndexActionModelIsComplete(Product[] products)
        {
            var controller = new HomeController();
            var mockRepo = new MockRepository();
            mockRepo.Products = products;
            controller.Repository = mockRepo;

            var model = (controller.Index() as ViewResult)?.ViewData.Model as IEnumerable<Product>;

            Assert.Equal(products, model,
                Comparer.Get<Product>((a, b) => a.Name == b.Name && a.Price == b.Price));

        }
    }
}
