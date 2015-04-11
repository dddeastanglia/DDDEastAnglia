using System;
using System.Net.Mail;
using DDDEastAnglia.Helpers.Email;
using DDDEastAnglia.Helpers.Email.SendGrid;
using NSubstitute;
using NUnit.Framework;
using MailMessage = DDDEastAnglia.Services.Messenger.Email.MailMessage;

namespace DDDEastAnglia.Tests.Helpers.Email.SendGrid
{
    [TestFixture]
    public class SendGridPostman_Should
    {
        [Test]
        public void ThrowAnExceptionWhenConstructed_WhenTheSuppliedMailHostSettingsIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new SendGridPostman(null, Substitute.For<IRenderer>()));
        }

        [Test]
        public void ThrowAnExceptionWhenConstructed_WhenTheSuppliedRendererIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new SendGridPostman(Substitute.For<IMailHostSettingsProvider>(), null));
        }
    }
}
