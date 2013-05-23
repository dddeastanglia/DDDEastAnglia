using System;
using DDDEastAnglia.Helpers.Email.SendGrid;
using NUnit.Framework;

namespace DDDEastAnglia.Tests.Helpers.Email.SendGrid
{
    [TestFixture]
    public class SendGridMessageWrapper_Should
    {
        [Test]
        public void ThrowAnExceptionWhenConstructed_WhenTheSuppliedSendGridIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new SendGridMessageWrapper(null));
        }
    }
}
