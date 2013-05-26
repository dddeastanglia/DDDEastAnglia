using System;
using System.Web.Mvc;
using DDDEastAnglia.DataAccess;
using DDDEastAnglia.Models;

namespace DDDEastAnglia.Controllers
{
    public class NavigationBarController : Controller
    {
        private const string DefaultEventName = "DDDEA2013";
        private readonly IConferenceRepository conferenceRepository;

        public NavigationBarController() : this(Factory.GetConferenceRepository())
        {}

        public NavigationBarController(IConferenceRepository conferenceRepository)
        {
            if (conferenceRepository == null)
            {
                throw new ArgumentNullException("conferenceRepository");
            }

            this.conferenceRepository = conferenceRepository;
        }

        public ActionResult Index()
        {
            bool userIsAdmin = User.IsInRole("Administrator");
            var conference = conferenceRepository.GetByEventShortName(DefaultEventName);
            bool isRegistrationOpen = conference.CanRegister();
            bool isAgendaPublished = conference.CanPublishAgenda();

            var model = new NavigationMenuViewModel
                {
                            IsUserAnAdministrator = userIsAdmin,
                            IsAgendaPublished = isAgendaPublished,
                            IsRegistrationOpen = isRegistrationOpen
                };
            return PartialView(model);
        }
    }
}