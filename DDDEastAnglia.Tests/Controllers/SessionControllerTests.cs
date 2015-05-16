using System.Net.Mail;
using DDDEastAnglia.Controllers;
using DDDEastAnglia.DataAccess;
using DDDEastAnglia.Domain;
using DDDEastAnglia.Helpers;
using DDDEastAnglia.Models;
using DDDEastAnglia.Services.Messenger.Email;
using NSubstitute;
using NUnit.Framework;
using System.Web.Mvc;
using DDDEastAnglia.Services.Messenger.Email.Templates;
using MailMessage = DDDEastAnglia.Services.Messenger.Email.MailMessage;

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

        [TestFixture]
        public class EmailTests
        {
            private static readonly IPostman postman = Substitute.For<IPostman>();

            [Test]
            public void ShouldEmailTheUserTheirSubmission_WhenCreating()
            {
                var bob = new UserProfile { Name = "Bob", EmailAddress = "bob@example.com", UserName = "bob" };
                var session = new Session
                {
                    Title = "Bob's awesome session",
                    Abstract = "This is going to be awesome!"
                };

                var controller = new SessionControllerBuilder()
                    .WithPostman(postman)
                    .WhenSubmissionsAreOpen()
                    .ForUser(bob)
                    .Submitting(session)
                    .Build();

                controller.Create(session);

                var expectedMailMessage = FromTemplate(SessionCreatedMailTemplate.Create(session), bob);
                postman.Received().Deliver(expectedMailMessage);
            }

            [Test]
            public void ShouldEmailTheUserTheirUpdatedSubmission_WhenEditing()
            {
                var bob = new UserProfile { Name = "Bob", EmailAddress = "bob@example.com", UserName = "bob" };
                var session = new Session
                {
                    Title = "Bob's even more awesome session",
                    Abstract = "Just wait until you see it!",
                    SpeakerUserName = bob.UserName,
                    SessionId = 1
                };

                var controller = new SessionControllerBuilder()
                    .WithPostman(postman)
                    .ForUser(bob)
                    .Updating(session)
                    .Build();

                controller.Edit(bob.UserName, session);

                var expectedMailMessage = FromTemplate(SessionUpdatedMailTemplate.Create(session), bob);
                postman.Received().Deliver(expectedMailMessage);
            }

            private static MailMessage FromTemplate(IMailTemplate mailTemplate, UserProfile userProfile)
            {
                return new MailMessage
                {
                    To = new MailAddress(userProfile.EmailAddress, userProfile.Name),
                    From = new MailAddress("admin@dddeastanglia.com", "DDD East Anglia"),
                    Subject = mailTemplate.RenderSubjectLine(),
                    Body = mailTemplate.RenderBody()
                };
            }
        }

        private class SessionControllerBuilder
        {
            private IConferenceLoader conferenceLoader;
            private IUserProfileRepository userProfileRepository;
            private ISessionRepository sessionRepository;
            private ISessionSorter sessionSorter;
            private IPostman postman;
            private UserProfile user;

            public SessionControllerBuilder()
            {
                conferenceLoader = Substitute.For<IConferenceLoader>();
                userProfileRepository = Substitute.For<IUserProfileRepository>();
                sessionRepository = Substitute.For<ISessionRepository>();
                sessionSorter = Substitute.For<ISessionSorter>();
                postman = Substitute.For<IPostman>();
            }

            public SessionControllerBuilder WithPostman(IPostman newPostman)
            {
                postman = newPostman;
                return this;
            }

            public SessionControllerBuilder WhenSubmissionsAreOpen()
            {
                var conference = Substitute.For<IConference>();
                conference.CanSubmit().Returns(true);
                conferenceLoader.LoadConference().Returns(conference);

                return this;
            }

            public SessionControllerBuilder ForUser(UserProfile newUser)
            {
                user = newUser;
                userProfileRepository.GetUserProfileByUserName(newUser.UserName).Returns(newUser);

                return this;
            }

            public SessionControllerBuilder Submitting(Session session)
            {
                sessionRepository.AddSession(session).Returns(session);
                return this;
            }

            public SessionControllerBuilder Updating(Session session)
            {
                sessionRepository.Get(session.SessionId).Returns(session);
                return this;
            }

            public SessionController Build()
            {
                var sessionController = new SessionController(
                    conferenceLoader,
                    userProfileRepository,
                    sessionRepository,
                    sessionSorter,
                    new EmailMessengerFactory(postman));

                sessionController.SetupWithAuthenticatedUser(user);

                return sessionController;
            }
        }

        private static class SessionControllerFactory
        {
            public static SessionController Create(IConference conference)
            {
                var conferenceLoader = Substitute.For<IConferenceLoader>();
                conferenceLoader.LoadConference().Returns(conference);
                var controller = new SessionController(conferenceLoader, Substitute.For<IUserProfileRepository>(), Substitute.For<ISessionRepository>(), Substitute.For<ISessionSorter>(), new EmailMessengerFactory(Substitute.For<IPostman>()));
                return controller;
            }

            public static SessionController Create(Session session)
            {
                var sessionRepository = Substitute.For<ISessionRepository>();
                sessionRepository.Get(session.SessionId).Returns(session);
                var controller = new SessionController(Substitute.For<IConferenceLoader>(), Substitute.For<IUserProfileRepository>(), sessionRepository, Substitute.For<ISessionSorter>(), new EmailMessengerFactory(Substitute.For<IPostman>()));
                return controller;
            }
        }
    }
}
