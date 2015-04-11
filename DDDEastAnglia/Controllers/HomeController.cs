using System;
using System.Linq;
using System.Web.Mvc;
using DDDEastAnglia.DataAccess;
using DDDEastAnglia.Filters;
using DDDEastAnglia.Models;

namespace DDDEastAnglia.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConferenceLoader conferenceLoader;
        private ISponsorRepository sponsorRepository;
        private ISponsorSorter sponsorSorter;

        public HomeController(IConferenceLoader conferenceLoader, ISponsorRepository sponsorRepository, ISponsorSorter sponsorSorter)
        {
            if (conferenceLoader == null)
            {
                throw new ArgumentNullException("conferenceLoader");
            }

            this.conferenceLoader = conferenceLoader;
            this.sponsorRepository = sponsorRepository;
            this.sponsorSorter = sponsorSorter;
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

        public ActionResult Sponsors()
        {
            var sponsors = sponsorSorter.Sort(sponsorRepository.GetAllSponsors())
                .Select(x => new SponsorModel { Name = x.Name, SponsorId = x.SponsorId, Url = x.Url });
            return View(sponsors);
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
