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
            var conference = conferenceRepository.GetByEventShortName(DefaultEventName);

            var model = new NavigationMenuViewModel
                {
                            IsUserAnAdministrator = User.IsInRole("Administrator"),
                            IsAgendaPublished = conference.CanPublishAgenda(),
                            IsRegistrationOpen = conference.CanRegister()
                };
            return PartialView(model);
        }
    }
}