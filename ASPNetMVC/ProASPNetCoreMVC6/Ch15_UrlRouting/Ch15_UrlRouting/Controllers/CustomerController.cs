using Ch15_UrlRouting.Models;
using Microsoft.AspNetCore.Mvc;

namespace Ch15_UrlRouting.Controllers
{
    public class CustomerController : Controller
    {
        public IActionResult Index()
           => View("Result", new Result { Controller = nameof(CustomerController), Action = nameof(Index) });

        public IActionResult List()
           => View("Result", new Result { Controller = nameof(CustomerController), Action = nameof(List) });
    }
}
