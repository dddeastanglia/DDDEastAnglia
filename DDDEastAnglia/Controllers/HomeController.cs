using System;
using System.Web.Mvc;
using DDDEastAnglia.DataAccess;
using DDDEastAnglia.Filters;
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
            var conference = conferenceLoader.LoadConference();

            if (!conference.CanRegister())
            {
                return RedirectToAction("Index");
            }

            return View();
        }

        public ActionResult Team()
        {
            return View();
        }

        //public ActionResult Sponsors()
        //{
        //    return View();
        //}

        public ActionResult About()
        {
            var conference = conferenceLoader.LoadConference();
            bool showSessionSubmissionLink = conference.CanSubmit();
            return View(new AboutViewModel{ShowSessionSubmissionLink = showSessionSubmissionLink});
        }

        public ActionResult Register()
        {
            var conference = conferenceLoader.LoadConference();

            if (!conference.CanRegister())
            {
                return RedirectToAction("Index");
            }

            return View();
        }

        public ActionResult Agenda()
        {
            var conference = conferenceLoader.LoadConference();

            if (!conference.CanPublishAgenda())
            {
                return RedirectToAction("Index");
            }

            return View();
        }

        [AllowedWhenConferenceIsInPreview]
        public ActionResult Preview()
        {
            var conference = conferenceLoader.LoadConference();

            if (!conference.IsPreview())
            {
                return new RedirectResult("~/");
            }

            return View();
        }

        [AllowedWhenConferenceIsClosed]
        public ActionResult Closed()
        {
            var conference = conferenceLoader.LoadConference();

            if (!conference.IsClosed())
            {
                return new RedirectResult("~/");
            }

            return View();
        }
    }
}
