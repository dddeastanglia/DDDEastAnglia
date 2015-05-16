using DDDEastAnglia.Models;
using DDDEastAnglia.Services.Messenger.Email;
using NSubstitute;
using NUnit.Framework;
using System.Web.Mvc;
using DDDEastAnglia.Services.Messenger.Email.Templates;
using DDDEastAnglia.Tests.Builders;
using MailMessage = DDDEastAnglia.Tests.Helpers.Email.MailMessage;

namespace DDDEastAnglia.Tests.Controllers
{
    [TestFixture]
    public sealed class SessionControllerTests
    {
        [Test]
        public void CannotSeeSessionList_WhenTheConferenceSaysThatSessionsCannotBeShown()
        {
            var controller = new SessionControllerBuilder().Build();

            var result = controller.Index();

            Assert.That(result, Is.InstanceOf<HttpNotFoundResult>());
        }

        [Test]
        public void CannotSubmitASession_WhenSessionSubmissionIsClosed()
        {
            var controller = new SessionControllerBuilder().Build();

            var result = controller.Create(new Session());

            Assert.That(result, Is.InstanceOf<HttpUnauthorizedResult>());
        }

        [Test]
        public void CannotBeginToEditASession_WhenTheSessionBelongsToAnotherUser()
        {
            var session = new Session { SessionId = 1, SpeakerUserName = "bob" };
            var controller = new SessionControllerBuilder().Updating(session).Build();

            var result = controller.Edit("fred", 1);

            Assert.That(result, Is.InstanceOf<HttpUnauthorizedResult>());
        }

        [Test]
        public void CannotSaveAnEditToASession_WhenTheSessionBelongsToAnotherUser()
        {
            var session = new Session { SessionId = 1, SpeakerUserName = "bob" };
            var controller = new SessionControllerBuilder().Updating(session).Build();

            var result = controller.Edit("fred", session);

            Assert.That(result, Is.InstanceOf<HttpUnauthorizedResult>());
        }

        [Test]
        public void CannotBeginToDeleteASession_WhenTheSessionBelongsToAnotherUser()
        {
            var session = new Session { SessionId = 1, SpeakerUserName = "bob" };
            var controller = new SessionControllerBuilder().Updating(session).Build();

            var result = controller.Delete("fred", 1);

            Assert.That(result, Is.InstanceOf<HttpUnauthorizedResult>());
        }

        [Test]
        public void CannotConfirmToDeleteASession_WhenTheSessionBelongsToAnotherUser()
        {
            var session = new Session { SessionId = 1, SpeakerUserName = "bob" };
            var controller = new SessionControllerBuilder().Updating(session).Build();

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

                var expectedMailMessage = MailMessage.FromTemplate(SessionCreatedMailTemplate.Create(session), bob);
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

                var expectedMailMessage = MailMessage.FromTemplate(SessionUpdatedMailTemplate.Create(session), bob);
                postman.Received().Deliver(expectedMailMessage);
            }
        }
    }
}
