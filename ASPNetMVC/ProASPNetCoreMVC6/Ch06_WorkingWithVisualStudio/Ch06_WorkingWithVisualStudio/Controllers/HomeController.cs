using Ch06_WorkingWithVisualStudio.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Ch06_WorkingWithVisualStudio.Controllers
{
    public class HomeController : Controller
    {
        public IRepository Repository = Models.Repository.SharedRepository;

        public ViewResult Index()
        {
            return base.View(Repository.Products);
        }

        [HttpGet]
        public IActionResult AddProduct() => View(new Product());

        [HttpPost]
        public IActionResult AddProduct(Product p)
        {
            Repository.AddProduct(p);
            return RedirectToAction("Index");
        }
        
    }
}
