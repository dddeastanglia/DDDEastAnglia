using System;
using DDDEastAnglia.Helpers.AppSettings;

namespace DDDEastAnglia.Helpers.LoginMethods
{
    public class LoginMethodIconProvider
    {
        public const string AppSettingsKeyPrefix = "OAuthLoginIcon.";
        public const string UnknownProviderKeyName = "unknown";

        private readonly IAppSettingsProvider appSettingsProvider;

        public LoginMethodIconProvider(IAppSettingsProvider appSettingsProvider)
        {
            if (appSettingsProvider == null)
            {
                throw new ArgumentNullException("appSettingsProvider");
            }
            
            this.appSettingsProvider = appSettingsProvider;
        }

        public string GetIcon(string providerName)
        {
            string iconCss = appSettingsProvider.GetSetting(AppSettingsKeyPrefix + providerName);
            return string.IsNullOrWhiteSpace(iconCss) ? appSettingsProvider.GetSetting(AppSettingsKeyPrefix + UnknownProviderKeyName) : iconCss;
        }
    }
}
