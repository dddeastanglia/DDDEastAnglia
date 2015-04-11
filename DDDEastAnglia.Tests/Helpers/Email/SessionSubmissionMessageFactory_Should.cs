using DDDEastAnglia.Helpers.Email;
using DDDEastAnglia.Helpers.Email.SendGrid;
using DDDEastAnglia.Helpers.File;
using DDDEastAnglia.Models;
using NSubstitute;
using NUnit.Framework;
using System;
using DDDEastAnglia.Services.Messenger.Email;

namespace DDDEastAnglia.Tests.Helpers.Email
{
    [TestFixture]
    public class SessionSubmissionMessageFactory_Should
    {
        [Test]
        public void ThrowAnExceptionWhenConstructed_WhenTheSuppliedFileContentsProviderIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new SessionSubmissionMessageFactory(null));
        }

        [Test]
        public void SendAnEmailFromTheCorrectEmailAddressAndName()
        {
            var fileContentsProvider = Substitute.For<IFileContentsProvider>();
            var session = Substitute.For<Session>();
            var profile = new UserProfile { EmailAddress = "speaker@dddeastanglia.com" };
            SessionSubmissionMessageFactory factory = new SessionSubmissionMessageFactory(fileContentsProvider);

            MailMessage result = factory.Create("htmlTemplatePath", "textTemplatePath", session, profile, false);

            Assert.AreEqual(@"""DDD East Anglia"" <admin@dddeastanglia.com>", result.From.ToString());
        }

        [Test]
        public void SendAnEmailToTheCorrectEmail()
        {
            var fileContentsProvider = Substitute.For<IFileContentsProvider>();
            var session = Substitute.For<Session>();
            var profile = new UserProfile { EmailAddress = "speaker@dddeastanglia.com" };
            SessionSubmissionMessageFactory factory = new SessionSubmissionMessageFactory(fileContentsProvider);

            MailMessage result = factory.Create("htmlTemplatePath", "textTemplatePath", session, profile, false);

            Assert.AreEqual(profile.EmailAddress, result.To.ToString());
        }

        [Test]
        public void LoadTheHtmlContentsOfTheEmail_FromTheSpecifiedHtmlTemplatePath()
        {
            var fileContentsProvider = Substitute.For<IFileContentsProvider>();
            var session = Substitute.For<Session>();
            var profile = new UserProfile { EmailAddress = "speaker@dddeastanglia.com" };

            SessionSubmissionMessageFactory factory = new SessionSubmissionMessageFactory(fileContentsProvider);
            fileContentsProvider.GetFileContents(string.Empty).ReturnsForAnyArgs("file contents");

            factory.Create("htmlTemplatePath", "textTemplatePath", session, profile, false);

            fileContentsProvider.Received().GetFileContents("htmlTemplatePath");
        }

        [Test]
        public void LoadTheTextContentsOfTheEmail_FromTheSpecifiedTextTemplatePath()
        {
            var fileContentsProvider = Substitute.For<IFileContentsProvider>();
            var session = Substitute.For<Session>();
            var profile = new UserProfile { EmailAddress = "speaker@dddeastanglia.com" };

            SessionSubmissionMessageFactory factory = new SessionSubmissionMessageFactory(fileContentsProvider);
            fileContentsProvider.GetFileContents(string.Empty).ReturnsForAnyArgs("file contents");

            factory.Create("htmlTemplatePath", "textTemplatePath", session, profile, false);

            fileContentsProvider.Received().GetFileContents("textTemplatePath");
        }

        [Test]
        public void SubstituteTheSessionAbstract_IntoTheTextTemplate()
        {
            var fileContentsProvider = Substitute.For<IFileContentsProvider>();
            var session = new Session { Abstract = "abstract", Title = "title" };
            var profile = new UserProfile { EmailAddress = "speaker@dddeastanglia.com" };

            SessionSubmissionMessageFactory factory = new SessionSubmissionMessageFactory(fileContentsProvider);
            const string contentTemplate = "test {0} email";
            string content = string.Format(contentTemplate, session.Abstract);
            fileContentsProvider.GetFileContents("textTemplatePath").ReturnsForAnyArgs(content);

            MailMessage result = factory.Create("htmlTemplatePath", "textTemplatePath", session, profile, false);

            string expectedContent = string.Format(contentTemplate, "abstract");
            Assert.AreEqual(expectedContent, result.Body);
        }

        [Test]
        public void SendANewEmailForANewSession()
        {
            var fileContentsProvider = Substitute.For<IFileContentsProvider>();
            var session = new Session { Abstract = "abstract", Title = "title" };
            var profile = new UserProfile { EmailAddress = "speaker@dddeastanglia.com" };

            SessionSubmissionMessageFactory factory = new SessionSubmissionMessageFactory(fileContentsProvider);
            const string contentTemplate = "test {0} email";
            string content = string.Format(contentTemplate, session.Abstract);
            fileContentsProvider.GetFileContents("textTemplatePath").ReturnsForAnyArgs(content);

            MailMessage result = factory.Create("htmlTemplatePath", "textTemplatePath", session, profile, false);

            string expectedContent = "DDD East Anglia Session Submission: title";
            Assert.AreEqual(expectedContent, result.Subject);
        }

        [Test]
        public void SendAnUpdateEMailForAnUpdatedSession()
        {
            var fileContentsProvider = Substitute.For<IFileContentsProvider>();
            var session = new Session { Abstract = "abstract", Title = "title" };
            var profile = new UserProfile { EmailAddress = "speaker@dddeastanglia.com" };

            SessionSubmissionMessageFactory factory = new SessionSubmissionMessageFactory(fileContentsProvider);
            const string contentTemplate = "test {0} email";
            string content = string.Format(contentTemplate, session.Abstract);
            fileContentsProvider.GetFileContents("textTemplatePath").ReturnsForAnyArgs(content);

            MailMessage result = factory.Create("htmlTemplatePath", "textTemplatePath", session, profile, true);

            string expectedContent = "DDD East Anglia Updated Session: title";
            Assert.AreEqual(expectedContent, result.Subject);
        }
    }
}
