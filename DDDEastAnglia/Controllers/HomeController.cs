using System;
using System.Web.Mvc;
using DDDEastAnglia.DataAccess;
using DDDEastAnglia.Models;

namespace DDDEastAnglia.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConferenceLoader conferenceLoader;

        public HomeController(IConferenceLoader conferenceLoader)
        {
            if (conferenceLoader == null)
            {
                throw new ArgumentNullException("conferenceLoader");
            }
            
            this.conferenceLoader = conferenceLoader;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }

        public ActionResult Venue()
        {
            return View();
        }

        public ActionResult Accommodation()
        {
            return View();
        }

        public ActionResult Team()
        {
            return View();
        }

        public ActionResult Sponsors()
        {
            return View();
        }

        public ActionResult About()
        {
            var conference = conferenceLoader.LoadConference();
            bool showSessionSubmissionLink = conference.CanSubmit();
            return View(new AboutViewModel{ShowSessionSubmissionLink = showSessionSubmissionLink});
        }

        public ActionResult Register()
        {
            return View();
        }

        public ActionResult Agenda()
        {
            return View();
        }
    }
}
