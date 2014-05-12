using System.Web.Mvc;
using DDDEastAnglia.Controllers;
using DDDEastAnglia.DataAccess;
using DDDEastAnglia.Domain;
using DDDEastAnglia.Helpers;
using NSubstitute;
using NUnit.Framework;

namespace DDDEastAnglia.Tests.Controllers
{
    [TestFixture]
    public sealed class SessionControllerTests
    {
        [Test]
        public void CannotSeeSessionList_WhenTheConferenceSaysThatSessionsCannotBeShown()
        {
            var conference = Substitute.For<IConference>();
            conference.CanShowSessions().Returns(false);
            var conferenceLoader = Substitute.For<IConferenceLoader>();
            conferenceLoader.LoadConference().Returns(conference);
            var userProfileRepository = Substitute.For<IUserProfileRepository>();
            var sessionRepository = Substitute.For<ISessionRepository>();
            var sessionSorter = Substitute.For<ISessionSorter>();
            var controller = new SessionController(conferenceLoader, userProfileRepository, sessionRepository, sessionSorter);

            var result = controller.Index();

            Assert.That(result, Is.InstanceOf<HttpNotFoundResult>());
        }
    }
}
