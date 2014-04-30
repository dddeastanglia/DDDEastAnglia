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
            var conference = conferenceLoader.LoadConference();
            bool showVotingStats = conference.CanVote() || conference.CanPublishAgenda() || conference.CanRegister();
            return View(new MenuViewModel{ShowVotingStatsLink = showVotingStats});
        }
    }
}
