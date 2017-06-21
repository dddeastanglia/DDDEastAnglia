using System;
using System.Collections.Generic;
using System.Web.Mvc;
using DDDEastAnglia.Areas.Admin.Models;
using DDDEastAnglia.DataAccess;
using DDDEastAnglia.Filters;
using DDDEastAnglia.Models;

namespace DDDEastAnglia.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConferenceLoader conferenceLoader;
        private readonly IViewModelQuery<IEnumerable<SponsorModel>> sponsorsQuery;

        public HomeController(IConferenceLoader conferenceLoader, IViewModelQuery<IEnumerable<SponsorModel>> sponsorsQuery)
        {
            if (conferenceLoader == null)
            {
                throw new ArgumentNullException("conferenceLoader");
            }

            if (sponsorsQuery == null)
            {
                throw new ArgumentNullException("sponsorsQuery");
            }

            this.conferenceLoader = conferenceLoader;
            this.sponsorsQuery = sponsorsQuery;
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

        public ActionResult CodeOfConduct()
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

        public ActionResult Sponsors()
        {
            return View(sponsorsQuery.Get());
        }

        public ActionResult About()
        {
            var conference = conferenceLoader.LoadConference();
            var showSessionSubmissionLink = conference.CanSubmit();
            return View(new AboutViewModel { ShowSessionSubmissionLink = showSessionSubmissionLink });
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
