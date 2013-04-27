using System;
using DDDEastAnglia.Helpers.Email.Smtp;
using NUnit.Framework;

namespace DDDEastAnglia.Tests.Helpers.Email.Smtp
{
    [TestFixture]
    public class SmtpHostSettings_Should
    {
        [Test]
        public void ThrowAnExceptionWhenConstructed_WhenTheSuppliedHostIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new SmtpHostSettings(null));
        }

        [Test]
        public void UseTheStandardSmtpPort_WhenNoPortIsSupplied()
        {
            var hostSettings = new SmtpHostSettings("host");
            Assert.That(hostSettings.Port, Is.EqualTo(SmtpHostSettings.DefaultSmtpPort));
        }

        [Test]
        public void AllowNoUsernameToBeSet()
        {
            var hostSettings = new SmtpHostSettings("host");
            Assert.That(hostSettings.Username, Is.Null);
        }

        [Test]
        public void AllowNoPasswordToBeSet()
        {
            var hostSettings = new SmtpHostSettings("host");
            Assert.That(hostSettings.Password, Is.Null);
        }
    }
}
