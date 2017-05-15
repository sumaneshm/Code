using Ch06_WorkingWithVisualStudio.Models;
using System;
using Xunit;

namespace Ch06_WorkingWithVisualStudio.Tests
{
    public class ProductsTests
    {
        [Fact]
        public void CanChangeProductName()
        {
            var p = new Product { Name = "Test", Price = 100M };
            p.Name = "New Name";
            Assert.Equal("New Name", p.Name);
        }

        [Fact]
        public void CanChangeProductPrice()
        {
            var p = new Product { Name = "Test", Price = 200m };
            p.Price = 20m;
            Assert.Equal(20m, p.Price);
        }
    }
}
