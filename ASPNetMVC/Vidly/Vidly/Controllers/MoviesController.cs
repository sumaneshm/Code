using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vidly.Models;
using Vidly.ViewModels;

namespace Vidly.Controllers
{
    public class MoviesController : Controller
    {
        // GET: Movies
        public ActionResult Random()
        {
            var movie = new Movie { Id = 1, Name = "Shrek" };

            var customers = new List<Customer>
            {
                new Customer{Id=1,Name="Sumanesh"},
                new Customer{Id=2,Name="Saveetha"},
                new Customer{Id=3,Name="Aadhavan"},
                new Customer{Id=4,Name="Aghilan"},
            };

            //return Content("Hello World");
            //return new EmptyResult();
            //return RedirectToAction("Index", "Home", new { page=1, sort="name"});
            //ViewData["Movie"] = movie;
            //ViewBag.Movie = movie;
            //return View();


            var vm = new RandomMovieViewModel
            {
                Movie = movie,
                Customers = customers
            };

            return View(vm);



        }

        public ActionResult Edit(int id)
        {
            return Content("Id = " + id);
        }

        public ActionResult Index(int? pageIndex, string sortBy)
        {
            if(!pageIndex.HasValue)
            {
                pageIndex = 1;
            }
            if (string.IsNullOrWhiteSpace(sortBy))
            {
                sortBy = "Name";
            }

            return Content(string.Format("PageIndex={0},Name={1}", pageIndex, sortBy));
        }

        [Route("movies/released/{year}/{month:regex(\\d{2}):range(1,12)}")]
        public ActionResult ByReleaseDate(int year, int month)
        {
            return Content($"Year={year},Month={month}");
        }
    }
}