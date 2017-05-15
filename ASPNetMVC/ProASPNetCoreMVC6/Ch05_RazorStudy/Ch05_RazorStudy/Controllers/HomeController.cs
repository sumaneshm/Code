using Ch05_RazorStudy.Models;
using Microsoft.AspNetCore.Mvc;

namespace Ch05_RazorStudy.Controllers
{
    public class HomeController : Controller
    {
        public ViewResult Index()
        {
            return View(new Product { PropertyID = 1, Name = "T-Shirt", Description = "Beach T-Shirt, good for Summer", Category = "Dress", Price = 6.99m });
        }
    }
}
