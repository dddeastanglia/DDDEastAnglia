using System;
using DDDEastAnglia.Helpers.AppSettings;
using DDDEastAnglia.Helpers.Email.Smtp;
using NSubstitute;
using NUnit.Framework;

namespace DDDEastAnglia.Tests.Helpers.Email.Smtp
{
    [TestFixture]
    public class SmtpHostSettingsProvider_Should
    {
        [Test]
        public void ThrowAnExceptionWhenConstructed_WhenTheSuppliedAppSettingsProviderIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new SmtpHostSettingsProvider(null));
        }

        [Test]
        public void ReadTheHostSetting_FromTheAppSettings()
        {
            var appSettingsProvider = Substitute.For<IAppSettingsProvider>();
            appSettingsProvider.GetSetting(SmtpHostSettingsProvider.SmtpHostKey).Returns("host");
            appSettingsProvider.GetSetting(SmtpHostSettingsProvider.SmtpPortKey).Returns("25");
            var hostSettingsProvider = new SmtpHostSettingsProvider(appSettingsProvider);
            
            hostSettingsProvider.GetSettings();

            appSettingsProvider.Received().GetSetting(SmtpHostSettingsProvider.SmtpHostKey);
        }

        [Test]
        public void ReadThePortSetting_FromTheAppSettings()
        {
            var appSettingsProvider = Substitute.For<IAppSettingsProvider>();
            appSettingsProvider.GetSetting(SmtpHostSettingsProvider.SmtpPortKey).Returns("25");
            var hostSettingsProvider = new SmtpHostSettingsProvider(appSettingsProvider);
            
            hostSettingsProvider.GetSettings();

            appSettingsProvider.Received().GetSetting(SmtpHostSettingsProvider.SmtpPortKey);
        }
        
        [Test]
        public void ReadTheUsernameSetting_FromTheAppSettings()
        {
            var appSettingsProvider = Substitute.For<IAppSettingsProvider>();
            appSettingsProvider.GetSetting(SmtpHostSettingsProvider.SmtpPortKey).Returns("25");
            var hostSettingsProvider = new SmtpHostSettingsProvider(appSettingsProvider);
            
            hostSettingsProvider.GetSettings();

            appSettingsProvider.Received().GetSetting(SmtpHostSettingsProvider.SmtpUsernameKey);
        }

        [Test]
        public void ReadThePasswordSetting_FromTheAppSettings()
        {
            var appSettingsProvider = Substitute.For<IAppSettingsProvider>();
            appSettingsProvider.GetSetting(SmtpHostSettingsProvider.SmtpPortKey).Returns("25");
            var hostSettingsProvider = new SmtpHostSettingsProvider(appSettingsProvider);
            
            hostSettingsProvider.GetSettings();

            appSettingsProvider.Received().GetSetting(SmtpHostSettingsProvider.SmtpPasswordKey);
        }
    }
}