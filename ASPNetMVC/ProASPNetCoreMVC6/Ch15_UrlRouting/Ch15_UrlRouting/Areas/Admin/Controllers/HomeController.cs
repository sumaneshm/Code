using Ch15_UrlRouting.Areas.Admin.Models;
using Microsoft.AspNetCore.Mvc;

namespace Ch15_UrlRouting.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
    {
        private Person[] data = new[]
        {
            new Person{Name="Sumanesh",City="Salem"},
            new Person{Name="Saveetha",City="Thiruvannamalai" },
            new Person{Name="Aadhavan",City="Vellore" },
            new Person{Name="Aghilan",City="Singapore"}
        };

        public ViewResult Index() => View(data);
    }
}
