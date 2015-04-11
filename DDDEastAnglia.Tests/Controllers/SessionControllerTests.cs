using DDDEastAnglia.Controllers;
using DDDEastAnglia.DataAccess;
using DDDEastAnglia.Domain;
using DDDEastAnglia.Helpers;
using DDDEastAnglia.Helpers.Email;
using DDDEastAnglia.Models;
using NSubstitute;
using NUnit.Framework;
using System.Web.Mvc;
using DDDEastAnglia.Services.Messenger.Email;

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
            var controller = SessionControllerFactory.Create(conference);

            var result = controller.Index();

            Assert.That(result, Is.InstanceOf<HttpNotFoundResult>());
        }

        [Test]
        public void CannotSubmitASession_WhenSessionSubmissionIsClosed()
        {
            var conference = Substitute.For<IConference>();
            conference.CanSubmit().Returns(false);
            var controller = SessionControllerFactory.Create(conference);

            var result = controller.Create(new Session());

            Assert.That(result, Is.InstanceOf<HttpUnauthorizedResult>());
        }

        [Test]
        public void CannotBeginToEditASession_WhenTheSessionBelongsToAnotherUser()
        {
            var session = new Session { SessionId = 1, SpeakerUserName = "bob" };
            var controller = SessionControllerFactory.Create(session);

            var result = controller.Edit("fred", 1);

            Assert.That(result, Is.InstanceOf<HttpUnauthorizedResult>());
        }

        [Test]
        public void CannotSaveAnEditToASession_WhenTheSessionBelongsToAnotherUser()
        {
            var session = new Session { SessionId = 1, SpeakerUserName = "bob" };
            var controller = SessionControllerFactory.Create(session);

            var result = controller.Edit("fred", session);

            Assert.That(result, Is.InstanceOf<HttpUnauthorizedResult>());
        }

        [Test]
        public void CannotBeginToDeleteASession_WhenTheSessionBelongsToAnotherUser()
        {
            var session = new Session { SessionId = 1, SpeakerUserName = "bob" };
            var controller = SessionControllerFactory.Create(session);

            var result = controller.Delete("fred", 1);

            Assert.That(result, Is.InstanceOf<HttpUnauthorizedResult>());
        }

        [Test]
        public void CannotConfirmToDeleteASession_WhenTheSessionBelongsToAnotherUser()
        {
            var session = new Session { SessionId = 1, SpeakerUserName = "bob" };
            var controller = SessionControllerFactory.Create(session);

            var result = controller.DeleteConfirmed("fred", 1);

            Assert.That(result, Is.InstanceOf<HttpUnauthorizedResult>());
        }

        private static class SessionControllerFactory
        {
            public static SessionController Create(IConference conference)
            {
                var conferenceLoader = Substitute.For<IConferenceLoader>();
                conferenceLoader.LoadConference().Returns(conference);
                var controller = new SessionController(conferenceLoader, Substitute.For<IUserProfileRepository>(), Substitute.For<ISessionRepository>(), Substitute.For<ISessionSorter>(), Substitute.For<IPostman>(), Substitute.For<ISessionSubmissionMessageFactory>());
                return controller;
            }

            public static SessionController Create(Session session)
            {
                var sessionRepository = Substitute.For<ISessionRepository>();
                sessionRepository.Get(session.SessionId).Returns(session);
                var controller = new SessionController(Substitute.For<IConferenceLoader>(), Substitute.For<IUserProfileRepository>(), sessionRepository, Substitute.For<ISessionSorter>(), Substitute.For<IPostman>(), Substitute.For<ISessionSubmissionMessageFactory>());
                return controller;
            }
        }
    }
}
