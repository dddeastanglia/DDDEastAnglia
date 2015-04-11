using System;
using DDDEastAnglia.Helpers.Email.SendGrid;
using NUnit.Framework;

namespace DDDEastAnglia.Tests.Helpers.Email.SendGrid
{
    [TestFixture]
    public class SendGridEmailSender_Should
    {
        [Test]
        public void ThrowAnExceptionWhenConstructed_WhenTheSuppliedMailHostSettingsIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new SendGridPostman(null));
        }
    }
}
