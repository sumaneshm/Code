using ConfiguringApps.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace ConfiguringApps.Controllers
{
    public class HomeController : Controller
    {
        private readonly UptimeService _uptimeSvc;

        public HomeController(UptimeService uptime) => _uptimeSvc = uptime;

        public ViewResult Index() => View(
            new Dictionary<string, string>
            {
                ["Message"] = "Changed again. This is the Index Action",
                ["Uptime"] = _uptimeSvc.Uptime.ToString()
           });
    }
}
