using Microsoft.AspNetCore.Mvc;

namespace Ch04_LanguageFeatures.Controllers
{
    public class HomeController : Controller
    {
        public ViewResult Index()
        {
            return View(new[] { "C#", "Language", "Features" });
        }
    }
}
