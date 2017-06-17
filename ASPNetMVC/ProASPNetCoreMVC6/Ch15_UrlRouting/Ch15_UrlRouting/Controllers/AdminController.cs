using Ch15_UrlRouting.Models;
using Microsoft.AspNetCore.Mvc;

namespace Ch15_UrlRouting.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
           => View("Result", new Result { Controller = nameof(AdminController), Action = nameof(Index) });
    }
}
