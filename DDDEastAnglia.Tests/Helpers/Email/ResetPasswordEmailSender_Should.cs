using System;
using System.Net.Mail;
using DDDEastAnglia.Helpers.Email;
using DDDEastAnglia.Helpers.File;
using NSubstitute;
using NUnit.Framework;

namespace DDDEastAnglia.Tests.Helpers.Email
{
    [TestFixture]
    public class ResetPasswordEmailSender_Should
    {
        [Test]
        public void ThrowAnExceptionWhenConstructed_WhenTheSuppliedEmailSenderIsNull()
        {
            var messageFactory = Substitute.For<IMessageFactory>();
            var fileContentsProvider = Substitute.For<IFileContentsProvider>();
            Assert.Throws<ArgumentNullException>(() => new ResetPasswordEmailSender(null, messageFactory, fileContentsProvider));
        }
        
        [Test]
        public void ThrowAnExceptionWhenConstructed_WhenTheSuppliedMessageFactoryIsNull()
        {
            var emailSender = Substitute.For<IEmailSender>();
            var fileContentsProvider = Substitute.For<IFileContentsProvider>();
            Assert.Throws<ArgumentNullException>(() => new ResetPasswordEmailSender(emailSender, null, fileContentsProvider));
        }

        [Test]
        public void ThrowAnExceptionWhenConstructed_WhenTheSuppliedFileContentsProviderIsNull()
        {
            var emailSender = Substitute.For<IEmailSender>();
            var messageFactory = Substitute.For<IMessageFactory>();
            Assert.Throws<ArgumentNullException>(() => new ResetPasswordEmailSender(emailSender, messageFactory, null));
        }

        [Test]
        public void SendAnEmailFromTheCorrectEmailAddressAndName()
        {
            var emailSender = Substitute.For<IEmailSender>();
            var messageFactory = Substitute.For<IMessageFactory>();
            var fileContentsProvider = Substitute.For<IFileContentsProvider>();
            var sender = new ResetPasswordEmailSender(emailSender, messageFactory, fileContentsProvider);

            sender.SendEmail("htmlTemplatePath", "textTemplatePath", "test@example.com", "http://reset/Password.Url");

            messageFactory.Received()
                          .Create(new MailAddress(ResetPasswordEmailSender.FromEmailAddress, ResetPasswordEmailSender.FromEmailName), 
                                    Arg.Any<MailAddress>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>());
        }

        [Test]
        public void SendAnEmailToTheCorrectEmail()
        {
            var emailSender = Substitute.For<IEmailSender>();
            var messageFactory = Substitute.For<IMessageFactory>();
            var fileContentsProvider = Substitute.For<IFileContentsProvider>();
            var sender = new ResetPasswordEmailSender(emailSender, messageFactory, fileContentsProvider);

            sender.SendEmail("htmlTemplatePath", "textTemplatePath", "test@example.com", "http://reset/Password.Url");

            messageFactory.Received()
                          .Create(Arg.Any<MailAddress>(), new MailAddress("test@example.com"), 
                                    Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>());
        }

        [Test]
        public void LoadTheHtmlContentsOfTheEmail_FromTheSpecifiedHtmlTemplatePath()
        {
            var emailSender = Substitute.For<IEmailSender>();
            var messageFactory = Substitute.For<IMessageFactory>();
            var fileContentsProvider = Substitute.For<IFileContentsProvider>();
            var sender = new ResetPasswordEmailSender(emailSender, messageFactory, fileContentsProvider);
            fileContentsProvider.GetFileContents(null).ReturnsForAnyArgs("file contents");

            sender.SendEmail("htmlTemplatePath", "textTemplatePath", "test@example.com", "http://reset/Password.Url");

            fileContentsProvider.Received().GetFileContents("htmlTemplatePath");
        }

        [Test]
        public void LoadTheTextContentsOfTheEmail_FromTheSpecifiedTextTemplatePath()
        {
            var emailSender = Substitute.For<IEmailSender>();
            var messageFactory = Substitute.For<IMessageFactory>();
            var fileContentsProvider = Substitute.For<IFileContentsProvider>();
            var sender = new ResetPasswordEmailSender(emailSender, messageFactory, fileContentsProvider);
            fileContentsProvider.GetFileContents(null).ReturnsForAnyArgs("file contents");

            sender.SendEmail("htmlTemplatePath", "textTemplatePath", "test@example.com", "http://reset/Password.Url");

            fileContentsProvider.Received().GetFileContents("textTemplatePath");
        }
 
        [Test]
        public void SubstituteTheResetPasswordLink_IntoTheHtmlTemplate()
        {
            var emailSender = Substitute.For<IEmailSender>();
            var messageFactory = Substitute.For<IMessageFactory>();
            var fileContentsProvider = Substitute.For<IFileContentsProvider>();
            var sender = new ResetPasswordEmailSender(emailSender, messageFactory, fileContentsProvider);
            const string contentTemplate = "test {0} email";
            string content = string.Format(contentTemplate, ResetPasswordEmailSender.ResetLinkToken);
            fileContentsProvider.GetFileContents("htmlTemplatePath").ReturnsForAnyArgs(content);
            const string resetPasswordUrl = "http://reset/Password.Url";

            sender.SendEmail("htmlTemplatePath", "textTemplatePath", "test@example.com", resetPasswordUrl);

            string expectedContent = string.Format(contentTemplate, resetPasswordUrl);
            messageFactory.Received()
                          .Create(Arg.Any<MailAddress>(), Arg.Any<MailAddress>(), 
                                    Arg.Any<string>(), expectedContent, Arg.Any<string>());
        }
 
        [Test]
        public void SubstituteTheResetPasswordLink_IntoTheTextTemplate()
        {
            var emailSender = Substitute.For<IEmailSender>();
            var messageFactory = Substitute.For<IMessageFactory>();
            var fileContentsProvider = Substitute.For<IFileContentsProvider>();
            var sender = new ResetPasswordEmailSender(emailSender, messageFactory, fileContentsProvider);
            const string contentTemplate = "test {0} email";
            string content = string.Format(contentTemplate, ResetPasswordEmailSender.ResetLinkToken);
            fileContentsProvider.GetFileContents("textTemplatePath").ReturnsForAnyArgs(content);
            const string resetPasswordUrl = "http://reset/Password.Url";

            sender.SendEmail("htmlTemplatePath", "textTemplatePath", "test@example.com", resetPasswordUrl);

            string expectedContent = string.Format(contentTemplate, resetPasswordUrl);
            messageFactory.Received()
                          .Create(Arg.Any<MailAddress>(), Arg.Any<MailAddress>(), 
                                    Arg.Any<string>(), Arg.Any<string>(), expectedContent);
        }
    }
}
