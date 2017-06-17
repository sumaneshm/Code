using Microsoft.AspNetCore.Mvc;

namespace Ch15_UrlRouting.Controllers
{
    public class LegacyController : Controller
    {
        public ViewResult GetLegacyRoute(string legacyUrl) => View(nameof(GetLegacyRoute), legacyUrl);
    }
}
