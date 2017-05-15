using Ch04_LanguageFeatures.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Ch04_LanguageFeatures.Controllers
{
    public class HomeController : Controller
    {
        public ViewResult Index()
        {
            return View();
        }

        public ViewResult StringList()
        {
            return View(new string[] { "C#", "Language", "Fundamentals" });
        }

        public ViewResult RelatedProducts()
        {
            return View("ProductsList", Product.GetProducts());
        }

        public ViewResult FilteredProducts()
        {
            return View("ProductsList", Product.GetProducts().Filter(p => (p?.Price ?? 0) > 25));
        }

        public async Task<ViewResult> PerformLongRunningTask()
        {
            long? length = await LongRunningTask.GetPageLength();
            return View("StringList", new[] { $"Length : {length}" });
        }
        
        public ViewResult PerformSyncLongRunningTask()
        {
            var length = LongRunningTask.GetPageLength().Result;
            return View("StringList", new[] { $"{nameof(length)} is : {length}" });
        }
    }
}
