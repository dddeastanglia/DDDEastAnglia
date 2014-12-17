using DDDEastAnglia.Helpers.AppSettings;
using DDDEastAnglia.Helpers.LoginMethods;
using NSubstitute;
using NUnit.Framework;

namespace DDDEastAnglia.Tests.Helpers.LoginMethods
{
    [TestFixture]
    public sealed class LoginMethodIconProviderTests
    {
        [Test]
        public void GetIcon_ReturnsTheConfiguredIconForOAuthProvider()
        {
            var appSettingsProvider = Substitute.For<IAppSettingsProvider>();
            appSettingsProvider.GetSetting(LoginMethodIconProvider.AppSettingsKeyPrefix + "test").Returns("test-icon");
            var iconProvider = new LoginMethodIconProvider(appSettingsProvider);

            string icon = iconProvider.GetIcon("test");

            Assert.That(icon, Is.EqualTo("test-icon"));
        }

        [Test]
        public void GetIcon_ReturnsTheUnknownIconForAnUnknownOAuthProvider()
        {
            var appSettingsProvider = Substitute.For<IAppSettingsProvider>();
            appSettingsProvider.GetSetting(LoginMethodIconProvider.AppSettingsKeyPrefix + LoginMethodIconProvider.UnknownProviderKeyName).Returns("unknown-icon");
            var iconProvider = new LoginMethodIconProvider(appSettingsProvider);

            string icon = iconProvider.GetIcon("test");

            Assert.That(icon, Is.EqualTo("unknown-icon"));
        }
    }
}
