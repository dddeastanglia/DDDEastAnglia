using System;
using System.Security.Principal;
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
            var conferenceRepository = Substitute.For<IConferenceRepository>();
            var urlHelperFactory = Substitute.For<IUrlHelperFactory>();
            Assert.Throws<ArgumentNullException>(() => new NavigationBarController(conferenceRepository, null, urlHelperFactory));
        }

        [Test]
        public void ThrowAnException_IfConstructedWithANullUrlHelperFactory()
        {
            var conferenceRepository = Substitute.For<IConferenceRepository>();
            var menuStateFactory = Substitute.For<IMenuStateFactory>();
            Assert.Throws<ArgumentNullException>(() => new NavigationBarController(conferenceRepository, menuStateFactory, null));
        }

        [Test]
        public void SetTheAdminLinkToNotVisible_WhenTheUserIsNotInTheAdministratorRole()
        {
            var conference = Substitute.For<IConference>();
            var user = new GenericPrincipal(new GenericIdentity("bob"), new string[0]);
            var controller = CreateController(conference, user);

            var result = (PartialViewResult) controller.Index();

            var model = (NavigationMenuViewModel) result.Model;
            var adminLink = model.Links.Single(l => l.LinkText == "Admin");
            Assert.IsFalse(adminLink.IsVisible);
        }

        [Test]
        public void SetTheAdminLinkToVisible_WhenTheUserIsInTheAdministratorRole()
        {
            var conference = Substitute.For<IConference>();
            var user = new GenericPrincipal(new GenericIdentity("bob"), new[] {"administrator"});
            var controller = CreateController(conference, user);

            var result = (PartialViewResult) controller.Index();

            var model = (NavigationMenuViewModel) result.Model;
            var adminLink = model.Links.Single(l => l.LinkText == "Admin");
            Assert.IsTrue(adminLink.IsVisible);
        }

        [Test]
        public void SetTheRegistrationLinkToNotVisible_WhenTheEventReportsThatRegistrationIsNotOpen()
        {
            var conference = Substitute.For<IConference>();
            conference.CanRegister().Returns(false);
            var controller = CreateController(conference);

            var result = (PartialViewResult) controller.Index();

            var model = (NavigationMenuViewModel) result.Model;
            var registrationLink = model.Links.Single(l => l.LinkText == "Register");
            Assert.IsFalse(registrationLink.IsVisible);
        }

        [Test]
        public void SetTheRegistrationLinkToVisible_WhenTheEventReportsThatRegistrationIsOpen()
        {
            var conference = Substitute.For<IConference>();
            conference.CanRegister().Returns(true);
            var controller = CreateController(conference);

            var result = (PartialViewResult) controller.Index();

            var model = (NavigationMenuViewModel) result.Model;
            var registrationLink = model.Links.Single(l => l.LinkText == "Register");
            Assert.IsTrue(registrationLink.IsVisible);
        }

        [Test]
        public void SetTheAgendaLinkToNotVisible_WhenTheEventReportsThatTheAgendaIsNotPublished()
        {
            var conference = Substitute.For<IConference>();
            conference.CanPublishAgenda().Returns(false);
            var controller = CreateController(conference);

            var result = (PartialViewResult) controller.Index();

            var model = (NavigationMenuViewModel) result.Model;
            var agendaLink = model.Links.Single(l => l.LinkText == "Agenda");
            Assert.IsFalse(agendaLink.IsVisible);
        }

        [Test]
        public void SetTheAgendaLinkToVisible_WhenTheEventReportsThatTheAgendaIsPublished()
        {
            var conference = Substitute.For<IConference>();
            conference.CanPublishAgenda().Returns(true);
            var controller = CreateController(conference);

            var result = (PartialViewResult) controller.Index();

            var model = (NavigationMenuViewModel) result.Model;
            var agendaLink = model.Links.Single(l => l.LinkText == "Agenda");
            Assert.IsFalse(agendaLink.IsVisible);
        }

        private NavigationBarController CreateController(IConference conference)
        {
            var user = new GenericPrincipal(new GenericIdentity("bob"), new string[0]);
            return CreateController(conference, user);
        }

        private NavigationBarController CreateController(IConference conference, IPrincipal user)
        {
            var conferenceRepository = Substitute.For<IConferenceRepository>();
            conferenceRepository.GetByEventShortName(Arg.Any<string>()).Returns(conference);

            var menuStateFactory = Substitute.For<IMenuStateFactory>();
            var urlHelperFactory = Substitute.For<IUrlHelperFactory>();

            var controllerContext = Substitute.For<ControllerContext>();
            controllerContext.HttpContext.User.Returns(user);
            controllerContext.RequestContext.Returns(new RequestContext());
            controllerContext.ParentActionViewContext.RouteData.Returns(new RouteData());
            return new NavigationBarController(conferenceRepository, menuStateFactory, urlHelperFactory) {ControllerContext = controllerContext};
        }
    }
}
