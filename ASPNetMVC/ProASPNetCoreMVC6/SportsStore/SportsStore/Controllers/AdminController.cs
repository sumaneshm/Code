using Microsoft.AspNetCore.Mvc;
using SportsStore.Models;

namespace SportsStore.Controllers
{
    public class AdminController : Controller
    {
        private IProductRepository _repo;

        public AdminController(IProductRepository repository)
        {
            _repo = repository;
        }

        public ViewResult Index => View(_repo);
    }
}
