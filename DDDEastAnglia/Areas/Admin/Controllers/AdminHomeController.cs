using System;
using System.Web.Mvc;
using DDDEastAnglia.Areas.Admin.Models;
using DDDEastAnglia.DataAccess;

namespace DDDEastAnglia.Areas.Admin.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class AdminHomeController : Controller
    {
        private readonly IConferenceLoader conferenceLoader;

        public AdminHomeController(IConferenceLoader conferenceLoader)
        {
            if (conferenceLoader == null)
            {
                throw new ArgumentNullException("conferenceLoader");
            }
            
            this.conferenceLoader = conferenceLoader;
        }

        public ActionResult Index()
        {
            var menuViewModel = CreateMenuViewModel();
            return View(menuViewModel);
        }

        public ActionResult BannerLinks()
        {
            var menuViewModel = CreateMenuViewModel();
            return View(menuViewModel);
        }

        private MenuViewModel CreateMenuViewModel()
        {
            var conference = conferenceLoader.LoadConference();
            bool showVotingStats = conference.CanVote() || conference.CanPublishAgenda() || conference.CanRegister();
            var menuViewModel = new MenuViewModel {ShowVotingStatsLink = showVotingStats};
            return menuViewModel;
        }
    }
}
