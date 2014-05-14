using System.Web.Mvc;
using DDDEastAnglia.Controllers;
using DDDEastAnglia.DataAccess;
using DDDEastAnglia.Domain;
using DDDEastAnglia.Helpers.Sessions;
using NSubstitute;
using NUnit.Framework;

namespace DDDEastAnglia.Tests.Controllers
{
    [TestFixture]
    public sealed class SpeakerControllerTests
    {
        [Test]
        public void CannotSeeSpeakerList_WhenTheConferenceSaysThatSpeakersCannotBeShown()
        {
            var conference = Substitute.For<IConference>();
            conference.CanShowSpeakers().Returns(false);
            var conferenceLoader = Substitute.For<IConferenceLoader>();
            conferenceLoader.LoadConference().Returns(conference);
            var sessionLoaderFactory = Substitute.For<ISessionLoaderFactory>();
            var userProfileRepository = Substitute.For<IUserProfileRepository>();
            var userProfileFilterFactory = Substitute.For<IUserProfileFilterFactory>();
            var controller = new SpeakerController(conferenceLoader, sessionLoaderFactory, userProfileRepository, userProfileFilterFactory);

            var result = controller.Index();

            Assert.That(result, Is.InstanceOf<HttpNotFoundResult>());
        }
    }
}
