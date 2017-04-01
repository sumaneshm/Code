using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HelloWorld.Models;

namespace HelloWorld.Controllers
{
    public class ConferenceController : Controller
    {
        private readonly IConferenceRepository _repository;

        public ConferenceController(IConferenceRepository repository)
        {
            _repository = repository;
        }

        // GET: Conference
        public ActionResult Index()
        {
            IEnumerable<Conference> conferences = _repository.GetAll();
            ConferenceListModel[] models = MapToListModel(conferences);

            return View(models);
        }

        private ConferenceListModel[] MapToListModel(IEnumerable<Conference> conferences)
        {
            var conferenceListModels = conferences.Select(c => new ConferenceListModel
            {
                Name = c.Name,
                SessionCount = c.SessionCount,
                AttendeeCount = c.AttendeeCount
            });

            return conferenceListModels.ToArray();
        }
    }

    public class Conference
    {
        public int AttendeeCount { get; set; }
        public int SessionCount { get; set; }
        public string Name { get; set; }
    }

    public interface IConferenceRepository
    {
        IEnumerable<Conference> GetAll();
    }

    class ConferenceRepository : IConferenceRepository
    {
        public IEnumerable<Conference> GetAll()
        {
            return new List<Conference>
            {
                new Conference{Name="Introduction to ASP.Net MVC 5",AttendeeCount = 200, SessionCount = 1},
                new Conference{Name="RAZOR",AttendeeCount = 320, SessionCount = 2},
                new Conference{Name="Python, a new way of programming",AttendeeCount = 234, SessionCount = 3},
                new Conference{Name="Programming AngularJS line of applications",AttendeeCount = 452, SessionCount = 4},
            };
        }
    }
}