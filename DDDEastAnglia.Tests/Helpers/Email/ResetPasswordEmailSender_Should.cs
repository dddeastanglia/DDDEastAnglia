using DDDEastAnglia.Helpers.Email;
using DDDEastAnglia.Helpers.File;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Net.Mail;
using MailMessage = DDDEastAnglia.Helpers.Email.MailMessage;

namespace DDDEastAnglia.Tests.Helpers.Email
{
    [TestFixture]
    public class ResetPasswordEmailSender_Should
    {
        [Test]
        public void ThrowAnExceptionWhenConstructed_WhenTheSuppliedEmailSenderIsNull()
        {
            var fileContentsProvider = Substitute.For<IFileContentsProvider>();
            Assert.Throws<ArgumentNullException>(() => new ResetPasswordEmailSender(null, fileContentsProvider));
        }

        [Test]
        public void ThrowAnExceptionWhenConstructed_WhenTheSuppliedFileContentsProviderIsNull()
        {
            var emailSender = Substitute.For<IPostman>();
            Assert.Throws<ArgumentNullException>(() => new ResetPasswordEmailSender(emailSender, null));
        }

        [Test]
        public void LoadTheHtmlContentsOfTheEmail_FromTheSpecifiedHtmlTemplatePath()
        {
            var emailSender = Substitute.For<IPostman>();
            var fileContentsProvider = Substitute.For<IFileContentsProvider>();
            var sender = new ResetPasswordEmailSender(emailSender, fileContentsProvider);
            fileContentsProvider.GetFileContents(null).ReturnsForAnyArgs("file contents");

            sender.SendEmail("htmlTemplatePath", "textTemplatePath", "test@example.com", "http://reset/Password.Url");

            fileContentsProvider.Received().GetFileContents("htmlTemplatePath");
        }

        [Test]
        public void LoadTheTextContentsOfTheEmail_FromTheSpecifiedTextTemplatePath()
        {
            var emailSender = Substitute.For<IPostman>();
            var fileContentsProvider = Substitute.For<IFileContentsProvider>();
            var sender = new ResetPasswordEmailSender(emailSender, fileContentsProvider);
            fileContentsProvider.GetFileContents(null).ReturnsForAnyArgs("file contents");

            sender.SendEmail("htmlTemplatePath", "textTemplatePath", "test@example.com", "http://reset/Password.Url");

            fileContentsProvider.Received().GetFileContents("textTemplatePath");
        }

        [Test]
        public void SendTheExpectedEmail()
        {
            const string resetPasswordUrl = "http://reset/Password.Url";
            const string contentTemplate = "test {0} email";
            string expectedContent = string.Format(contentTemplate, resetPasswordUrl);
            var mailMessage = new MailMessage
            {
                From = new MailAddress(@"""DDD East Anglia"" <admin@dddeastanglia.com>"),
                To = new MailAddress("user@dddeastanglia.com"),
                Subject = "DDD East Anglia Password Reset Request",
                Html = expectedContent,
                Text = expectedContent
            };

            var emailSender = Substitute.For<IPostman>();
            var fileContentsProvider = Substitute.For<IFileContentsProvider>();
            var sender = new ResetPasswordEmailSender(emailSender, fileContentsProvider);
            string content = string.Format(contentTemplate, ResetPasswordEmailSender.ResetLinkToken);
            fileContentsProvider.GetFileContents("textTemplatePath").ReturnsForAnyArgs(content);

            sender.SendEmail("htmlTemplatePath", "textTemplatePath", "user@dddeastanglia.com", resetPasswordUrl);

            emailSender.Received()
                          .Deliver(mailMessage);
        }
    }
}
