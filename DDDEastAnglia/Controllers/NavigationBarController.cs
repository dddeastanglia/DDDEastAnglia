using System;
using System.Collections.Generic;
using System.Web.Mvc;
using DDDEastAnglia.DataAccess;
using DDDEastAnglia.Models;
using DDDEastAnglia.NavigationMenu;

namespace DDDEastAnglia.Controllers
{
    public class NavigationBarController : Controller
    {
        private const string DefaultEventName = "DDDEA2013";
        private readonly IConferenceRepository conferenceRepository;
        private readonly IMenuStateFactory menuStateFactory;
        private readonly IUrlHelperFactory urlHelperFactory;

        public NavigationBarController() : this(Factory.GetConferenceRepository(), new MenuStateFactory(), new UrlHelperFactory())
        {}

        public NavigationBarController(IConferenceRepository conferenceRepository, IMenuStateFactory menuStateFactory, IUrlHelperFactory urlHelperFactory)
        {
            if (conferenceRepository == null)
            {
                throw new ArgumentNullException("conferenceRepository");
            }

            if (menuStateFactory == null)
            {
                throw new ArgumentNullException("menuStateFactory");
            }

            if (urlHelperFactory == null)
            {
                throw new ArgumentNullException("urlHelperFactory");
            }
            
            this.conferenceRepository = conferenceRepository;
            this.menuStateFactory = menuStateFactory;
            this.urlHelperFactory = urlHelperFactory;
        }

        [ChildActionOnly]
        public ActionResult RenderMenu()
        {
            var conference = conferenceRepository.GetByEventShortName(DefaultEventName);
            var links = new List<NavigationMenuLinkViewModel>
                {
                    CreateLink("Home", "Home", "Index"), 
                    CreateLink("Sessions", "Session", "Index", () => !conference.CanPublishAgenda() && (conference.CanSubmit() || conference.CanVote())),
                    CreateLink("Speakers", "Speaker", "Index"),
                    CreateLink("Agenda", "Home", "Agenda", conference.CanPublishAgenda),
                    CreateLink("Register", "Home", "Register", conference.CanRegister),
                    CreateLink("New to DDD?", "Home", "About"),
                    CreateLink("Venue", "Home", "Venue"),
                    CreateLink("Accommodation", "Home", "Accommodation"),
                    CreateLink("Sponsors", "Home", "Sponsors"),
                    CreateLink("Team", "Home", "Team"),
                    CreateLink("Contact", "Home", "Contact"),
                    CreateLink("Admin", "AdminHome", "Index", () => User.IsInRole("Administrator"), new { area = "Admin" }),
                };

            var model = new NavigationMenuViewModel { Links = links };
            return PartialView(model);
        }

        private NavigationMenuLinkViewModel CreateLink(string linkText, string controllerName, string actionName, 
                                                        Func<bool> isVisible = null, object routeValues = null)
        {
            var urlHelper = urlHelperFactory.Create(ControllerContext.RequestContext);
            var menuState = menuStateFactory.Create(ControllerContext.ParentActionViewContext.RouteData);
            return new NavigationMenuLinkViewModel
                {
                            LinkText = linkText,
                            LinkUrl = urlHelper.Action(actionName, controllerName, routeValues),
                            IsActive = menuState.IsCurrentlySelectedItem(controllerName, actionName),
                            IsVisible = isVisible == null || isVisible()
                };
        }
    }
}
