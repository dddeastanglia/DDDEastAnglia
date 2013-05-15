using System;
using System.Security.Principal;
using System.Web.Mvc;
using DDDEastAnglia.Controllers;
using DDDEastAnglia.DataAccess;
using DDDEastAnglia.Domain;
using DDDEastAnglia.Models;
using NSubstitute;
using NUnit.Framework;

namespace DDDEastAnglia.Tests.Controllers
{
    [TestFixture]
    public sealed class NavigationBarControllerShould
    {
        [Test]
        public void ThrowAnException_IfConstructedWithANullConferenceRepository()
        {
            Assert.Throws<ArgumentNullException>(() => new NavigationBarController(null));
        }

        [Test]
        public void IndicateThatTheUserIsAnAdministrator_WhenTheUserIsInTheAdministratorRole()
        {
            var conference = Substitute.For<IConference>();
            var user = new GenericPrincipal(new GenericIdentity("bob"), new[] {"administrator"});
            var controller = CreateController(conference, user);

            var result = (PartialViewResult) controller.Index();

            var model = (NavigationMenuViewModel) result.Model;
            Assert.IsTrue(model.IsUserAnAdministrator);
        }

        [Test]
        public void IndicateThatRegistrationIsOpen_WhenTheEventReportsThatRegistrationIsOpen()
        {
            var conference = Substitute.For<IConference>();
            conference.CanRegister().Returns(true);
            var controller = CreateController(conference);

            var result = (PartialViewResult) controller.Index();

            var model = (NavigationMenuViewModel) result.Model;
            Assert.IsTrue(model.IsRegistrationOpen);
        }

        [Test]
        public void IndicateThatTheAgendaIsPublished_WhenTheEventReportsThatTheAgendaIsPublished()
        {
            var conference = Substitute.For<IConference>();
            conference.CanPublishAgenda().Returns(true);
            var controller = CreateController(conference);

            var result = (PartialViewResult) controller.Index();

            var model = (NavigationMenuViewModel) result.Model;
            Assert.IsTrue(model.IsAgendaPublished);
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

            var controllerContext = Substitute.For<ControllerContext>();
            controllerContext.HttpContext.User.Returns(user);
            return new NavigationBarController(conferenceRepository) {ControllerContext = controllerContext};
        }
    }
}
