using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HelloWorld.Controllers
{
    public class DynamicController : Controller
    {
        // GET: Dynamic
        public ActionResult Index(string text = "default value")
        {
            ViewBag.DisplayText = "Welcome to my first webpage!!!! \n" + text;

            return View();
        }

        [HttpPost]
        public ActionResult Goto(string gotoUrl)
        {
            return Redirect(gotoUrl);
        }
    }
}