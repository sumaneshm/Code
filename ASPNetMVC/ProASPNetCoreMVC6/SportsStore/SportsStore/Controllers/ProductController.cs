using Microsoft.AspNetCore.Mvc;
using SportsStore.Models;
using SportsStore.Models.ViewModels;
using System.Linq;

namespace SportsStore.Controllers
{
    public class ProductController : Controller
    {
        public int PageSize = 3;

        private IProductRepository repository;

        public ProductController(IProductRepository repo) => repository = repo;

        public ViewResult List(string category, int page = 1)
        {
            var filteredProducts = repository.Products.Where(p => category == null || p.Category == category);

            return View(new ProductsListViewModel
            {
                Products =
                     filteredProducts
                        .OrderBy(p => p.ProductID)
                        .Skip((page - 1) * PageSize)
                        .Take(PageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,

                    ItemsPerPage = PageSize,
                    TotalItems = filteredProducts.Count(),
                },
                CurrentCategory = category
            });
        }
    }
}
