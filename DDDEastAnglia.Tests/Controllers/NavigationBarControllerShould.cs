using System;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using DDDEastAnglia.Controllers;
using DDDEastAnglia.DataAccess;
using DDDEastAnglia.Domain;
using DDDEastAnglia.Models;
using DDDEastAnglia.NavigationMenu;
using NSubstitute;
using NUnit.Framework;
using System.Linq;

namespace DDDEastAnglia.Tests.Controllers
{
    [TestFixture]
    public sealed class NavigationBarControllerShould
    {
        [Test]
        public void ThrowAnException_IfConstructedWithANullConferenceRepository()
        {
            var menuStateFactory = Substitute.For<IMenuStateFactory>();
            var urlHelperFactory = Substitute.For<IUrlHelperFactory>();
            Assert.Throws<ArgumentNullException>(() => new NavigationBarController(null, menuStateFactory, urlHelperFactory));
        }

        [Test]
        public void ThrowAnException_IfConstructedWithANullMenuStateFactory()
        {
            var conferenceLoader = Substitute.For<IConferenceLoader>();
            var urlHelperFactory = Substitute.For<IUrlHelperFactory>();
            Assert.Throws<ArgumentNullException>(() => new NavigationBarController(conferenceLoader, null, urlHelperFactory));
        }

        [Test]
        public void ThrowAnException_IfConstructedWithANullUrlHelperFactory()
        {
            var conferenceLoader = Substitute.For<IConferenceLoader>();
            var menuStateFactory = Substitute.For<IMenuStateFactory>();
            Assert.Throws<ArgumentNullException>(() => new NavigationBarController(conferenceLoader, menuStateFactory, null));
        }

        [TestCase("Home")]
        [TestCase("New to DDD?")]
        [TestCase("Venue")]
        [TestCase("Accommodation")]
        [TestCase("Sponsors")]
        [TestCase("Team")]
        [TestCase("Contact")]
        public void IncludeAStandardMenuItemThatIsVisible(string linkText)
        {
            var controller = CreateController();

            var result = controller.RenderMenu();

            var link = FindLink(result, linkText);
            Assert.IsTrue(link.IsVisible);
        }

        [TestCase("Home", "Home", "Index")]
        [TestCase("Sessions", "Session", "Index")]
        [TestCase("Speakers", "Speaker", "Index")]
        [TestCase("Agenda", "Home", "Agenda")]
        [TestCase("Register", "Home", "Register")]
        [TestCase("New to DDD?", "Home", "About")]
        [TestCase("Venue", "Home", "Venue")]
        [TestCase("Accommodation", "Home", "Accommodation")]
        [TestCase("Sponsors", "Home", "Sponsors")]
        [TestCase("Team", "Home", "Team")]
        [TestCase("Contact", "Home", "Contact")]
        [TestCase("Admin", "AdminHome", "Index")]
        public void SetTheLinkThatMatchesTheCurrentOneToActive(string linkText, string controllerName, string actionName)
        {
            var controller = CreateController(currentControllerName: controllerName, currentActionName: actionName);

            var result = controller.RenderMenu();

            var link = FindLink(result, linkText);
            Assert.IsTrue(link.IsActive);
        }

        [Test]
        public void SetTheAdminLinkToNotVisible_WhenTheUserIsNotInTheAdministratorRole()
        {
            var controller = CreateController();

            var result = controller.RenderMenu();

            var adminLink = FindLink(result, "Admin");
            Assert.IsFalse(adminLink.IsVisible);
        }

        [Test]
        public void SetTheAdminLinkToVisible_WhenTheUserIsInTheAdministratorRole()
        {
            var controller = CreateController(userRoles: new[] {"administrator"});

            var result = controller.RenderMenu();

            var adminLink = FindLink(result, "Admin");
            Assert.IsTrue(adminLink.IsVisible);
        }

        [Test]
        public void SetTheRegistrationLinkToNotVisible_WhenTheEventReportsThatRegistrationIsNotOpen()
        {
            var controller = CreateController(conference => conference.CanRegister().Returns(false));

            var result = controller.RenderMenu();

            var registrationLink = FindLink(result, "Register");
            Assert.IsFalse(registrationLink.IsVisible);
        }

        [Test]
        public void SetTheRegistrationLinkToVisible_WhenTheEventReportsThatRegistrationIsOpen()
        {
            var controller = CreateController(conference => conference.CanRegister().Returns(true));

            var result = controller.RenderMenu();

            var registrationLink = FindLink(result, "Register");
            Assert.IsTrue(registrationLink.IsVisible);
        }

        [Test]
        public void SetTheAgendaLinkToNotVisible_WhenTheEventReportsThatTheAgendaIsNotPublished()
        {
            var controller = CreateController(conference => conference.CanPublishAgenda().Returns(false));

            var result = controller.RenderMenu();

            var agendaLink = FindLink(result, "Agenda");
            Assert.IsFalse(agendaLink.IsVisible);
        }

        [Test]
        public void SetTheSessionsLinkToVisible_WhenTheEventReportsThatSessionSubmissionIsOpen()
        {
            var controller = CreateController(conference => conference.CanSubmit().Returns(true));

            var result = controller.RenderMenu();

            var agendaLink = FindLink(result, "Sessions");
            Assert.IsTrue(agendaLink.IsVisible);
        }

        [Test]
        public void SetTheSessionsLinkToVisible_WhenTheEventReportsThatSessionVotingIsOpen()
        {
            var controller = CreateController(conference => conference.CanVote().Returns(true));

            var result = controller.RenderMenu();

            var agendaLink = FindLink(result, "Sessions");
            Assert.IsTrue(agendaLink.IsVisible);
        }

        [Test]
        public void SetTheSessionsLinkToNotVisible_WhenTheEventReportsThatTheAgendaIsPublished()
        {
            var controller = CreateController(conference => conference.CanPublishAgenda().Returns(true));

            var result = controller.RenderMenu();

            var agendaLink = FindLink(result, "Sessions");
            Assert.IsFalse(agendaLink.IsVisible);
        }

        [Test]
        public void SetTheAgendaLinkToVisible_WhenTheEventReportsThatTheAgendaIsPublished()
        {
            var controller = CreateController(conference => conference.CanPublishAgenda().Returns(true));

            var result = controller.RenderMenu();

            var agendaLink = FindLink(result, "Agenda");
            Assert.IsTrue(agendaLink.IsVisible);
        }

        private NavigationMenuLinkViewModel FindLink(ActionResult result, string linkText)
        {
            var partialResult =(PartialViewResult) result;
            var model = (NavigationMenuViewModel) partialResult.Model;
            var link = model.Links.Single(l => l.LinkText == linkText);
            return link;
        }

        private NavigationBarController CreateController(Action<IConference> conferenceSetupCallback = null, 
                                                            string[] userRoles = null, 
                                                            string currentControllerName = "some controller", 
                                                            string currentActionName = "some action")
        {
            var conference = Substitute.For<IConference>();

            if (conferenceSetupCallback != null)
            {
                conferenceSetupCallback(conference);
            }

            var user = new GenericPrincipal(new GenericIdentity("bob"), userRoles ?? new string[0]);

            var conferenceLoader = Substitute.For<IConferenceLoader>();
            conferenceLoader.LoadConference().Returns(conference);
            
            var routeData = new RouteData();
            routeData.Values.Add("controller", currentControllerName);
            routeData.Values.Add("action", currentActionName);

            var requestContext = new RequestContext {RouteData = routeData};

            var menuStateFactory = Substitute.For<IMenuStateFactory>();
            menuStateFactory.Create(Arg.Any<RouteData>()).Returns(new MenuState(routeData));
            var urlHelperFactory = Substitute.For<IUrlHelperFactory>();
            urlHelperFactory.Create(Arg.Any<RequestContext>()).Returns(new UrlHelper(requestContext));

            var controllerContext = new ControllerContext();
            routeData.DataTokens["ParentActionViewContext"] = new ViewContext {RouteData = routeData};
            controllerContext.RouteData = routeData;
            controllerContext.HttpContext = Substitute.For<HttpContextBase>();
            controllerContext.HttpContext.User.Returns(user);
            controllerContext.RequestContext = requestContext;
            return new NavigationBarController(conferenceLoader, menuStateFactory, urlHelperFactory) {ControllerContext = controllerContext};
        }
    }
}
