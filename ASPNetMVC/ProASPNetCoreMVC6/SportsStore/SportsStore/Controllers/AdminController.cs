using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SportsStore.Models;
using System.Linq;

namespace SportsStore.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private IProductRepository _repo;

        public AdminController(IProductRepository repository)
        {
            _repo = repository;
        }

        public ViewResult Index() => View(_repo.Products);

        [HttpPost]
        public IActionResult Edit(Product product)
        {
            if (!ModelState.IsValid)
            {
                return View("ProductSummary", product);
            }
            _repo.SaveProduct(product);

            if (product.ProductID != 0)
            {
                TempData["message"] = $"Product {product.ProductID} has been edited";
            }
            else
            {
                TempData["message"] = $"New Product has been created";
            }
            return RedirectToAction(nameof(Index));
        }

        public IActionResult AddNew() => View("ProductSummary", new Product());

        public IActionResult Edit(int productID) => View("ProductSummary", _repo.Products.FirstOrDefault(p => p.ProductID == productID));

        [HttpPost]
        public IActionResult Delete(int productID)
        {
            var delProd = _repo.Products.FirstOrDefault(p => p.ProductID == productID);

            if (delProd != null)
            {
                _repo.DeleteProduct(productID);

                TempData["message"] = $"Product ID: {productID} has been deleted";
            }
            else
            {
                TempData["message"] = $"Product ID: {productID} has not been found";
            }


            return RedirectToAction(nameof(Index));
        }
    }
}
