using Ch15_UrlRouting.Models;
using Microsoft.AspNetCore.Mvc;

namespace Ch15_UrlRouting.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
            => View("Result", new Result { Controller = nameof(HomeController), Action = nameof(Index) });

        public IActionResult CustomVariable(string id)
        {
            Result r = new Result { Controller = nameof(HomeController), Action = nameof(CustomVariable) };
            r.Data["id"] = id; // RouteData.Values["id"];
            r.Data["url"] = Url.Action("CustomVariable", "Home", new { id = 123 });
            return View("Result", r);
        }
    }
}
