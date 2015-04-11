using System;
using DDDEastAnglia.Helpers.Email;
using DDDEastAnglia.Helpers.Email.SendGrid;
using NSubstitute;
using NUnit.Framework;

namespace DDDEastAnglia.Tests.Helpers.Email.SendGrid
{
    [TestFixture]
    public class SendGridEmailSender_Should
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
